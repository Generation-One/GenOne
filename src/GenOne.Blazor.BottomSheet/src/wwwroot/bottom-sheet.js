
class BottomSheet {
    constructor(stops, passive, sensitivity, onClosedHandler) {
        this._onClosedHandler = onClosedHandler;
        this._hidden = true;
        this._passive = passive;
        this._height = 0;
        this._currentDragPosition = 0;
        this._startDragPosition = 0;
        this._startDragHeight = 0;
        this._checkContentPosition = false;
        this._inDrag = false;
        this._dragFinished = false;
        this._touched = false;
        this._windowHeight = window.innerHeight
            || document.documentElement.clientHeight
            || document.body.clientHeight;
        this._stops = stops;
        this._sensitivity = sensitivity;
        this._scrollPosition = true;
        this._sheetListenersAdded = false;
        this._stopPropagationCount = 0;
    }

    reinitialize() {
        this._header = document.querySelector('[data-bs-header]');
        this._content = document.querySelector('[data-bs-contents]');
        this._footer = document.querySelector('[data-bs-footer]');
        this._watch = document.querySelector('[data-bs-watch]');
        this._sheet = document.querySelector('[data-bs]');
        this._overlay = document.querySelector('[data-bs-overlay]');
    }

    addSheetListeners() {
        this._overlay.addEventListener("click", this.close.bind(this));
        this._sheet.addEventListener("touchend", this.onDragEnd.bind(this));
        this._sheet.addEventListener("mouseup", this.onDragEnd.bind(this));
        this._sheet.addEventListener("touchmove", this.onDragTouch.bind(this));
        this._sheet.addEventListener("mousemove", this.onDragMouse.bind(this));
    }

    addContentsListeners() {
        recursiveUpdate(this._header, '[data-bs-header]', value => {
            this._header = value;
            this._header.addEventListener("touchstart", this.onDragStartTouch.bind(this), { once: true, capture: true });
            this._header.addEventListener("mousedown", this.onDragStartMouse.bind(this), { once: true, capture: true });
        });

        recursiveUpdate(this._content, '[data-bs-contents]', value => {
            this._content = value;
            this._content.addEventListener("touchstart", this.onDragStartTouchContent.bind(this), { once: true });
            this._content.addEventListener("mousedown", this.onDragStartMouseContent.bind(this), { once: true });
        });

        recursiveUpdate(this._footer, '[data-bs-footer]', value => {
            this._footer = value;
            this._footer.addEventListener("touchstart", this.onDragStartTouch.bind(this), { once: true, capture: true });
            this._footer.addEventListener("mousedown", this.onDragStartMouse.bind(this), { once: true, capture: true });
        });
    }

    addWatchObserver() {
        recursiveUpdate(this._watch, '[data-bs-watch]', value => {
            this._watch = value;
            observeWatch(this._watch, x => this._scrollPosition = !x);
        });
    }

    onDragMouse(event) {
        this.onDragInternal(event.pageY)
        if (this._inDrag) { 
            this._stopPropagationCount++;
            event.preventDefault();
            event.stopPropagation();
        }
    }

    onDragTouch(event) {
        this.onDragInternal(event.touches[0].clientY)
        if (this._inDrag) {
            this._stopPropagationCount++;
            event.preventDefault();
            event.stopPropagation();
        }
    }

    onDragStartTouch(event) {
        this.onDragStart(event.touches[0].clientY, false)
    }

    onDragStartMouse(event) {
        this.onDragStart(event.pageY, false)
    }

    onDragStartTouchContent(event) {
        if (!this._touched && !this._inDrag) {
            this.onDragStart(event.touches[0].clientY, true)
        }
    }

    onDragStartMouseContent(event) {
        if (!this._touched && !this._inDrag) {
            this.onDragStart(event.pageY, true)
        }
    }

    open() {
        if(!this._hidden)
            return;

        this._hidden = false;
        this._height = this._stops[0];

        const update = () => {
            this.reinitialize();

            if (this._hidden)
                this._sheet.setAttribute("aria-hidden", "");
            else
                this._sheet.removeAttribute("aria-hidden");

            this._contents = document.querySelector('[data-bs-contents]');
            this._contents.style.height = this._height + "vh";

            setTimeout(() => {
                this.addSheetListeners();
                this.addContentsListeners();
                this.addWatchObserver();
            });
        }

        requestAnimationFrame(update);
    }

    updateStyle() {
        if (this._inDrag)
            this._sheet.setAttribute("data-bs-scroll", "");
        else
            this._sheet.removeAttribute("data-bs-scroll");

        this._contents.style.height = this._height + "vh";

        if (this._height == 100)
            this._contents.setAttribute("full-screen", "");
        else
            this._contents.removeAttribute("full-screen");

        if (this._hidden)
            this._sheet.setAttribute("aria-hidden", "");
        else
            this._sheet.removeAttribute("aria-hidden");
    }

    close() {
        return this.closeInner();
    }

    closeInner() {
        if(this._hidden)
            return;
        
        this._hidden = true;
        this._height = 0;

        this._contents.addEventListener("transitionend", () => {
            this._onClosedHandler.invokeMethodAsync('Callback')
        }, { once: true });

        requestAnimationFrame(() => {
            this.updateStyle();
        });
    }

    onDragStart(position, checkContentPosition) {
        if (this._passive)
            return;

        this._checkContentPosition = checkContentPosition;
        this._currentDragPosition = position;
        this._startDragPosition = position;
        this._startDragHeight = this._height;
        this._dragFinished = false;
        this._touched = true;
        this._stopPropagationCount = 0;

        const updateStylesInner = () => {
            this.updateStyle()

            if (!this._dragFinished)
                requestAnimationFrame(updateStylesInner);
        }

        requestAnimationFrame(updateStylesInner);
    }

    onDragEnd() {
        this._currentDragPosition = 0;
        this._dragFinished = true;
        this._touched = false;
        this._inDrag = false;

        if (this._passive)
            return;

        if (this._height < this._stops[0] - this._sensitivity) {
            this.closeInner();
        }
        else {
            this._height = this.getClosestStop(this._height);
            this.addContentsListeners();
        }

        this._sheet.removeAttribute("data-bs-scroll");
    }

    getClosestStop(goal) {
        if (this._stops.length == 1)
            return this._stops[0];

        return this._stops.reduce((prev, cur) => Math.abs(cur - goal) < Math.abs(prev - goal) ? cur : prev);
    }

    onDragInternal(position) {
        if (this._passive)
            return;

        if (!this._touched && !this._inDrag)
            return;

        var deltaY = this._currentDragPosition - position;

        if (this._checkContentPosition) {
            if (deltaY == 0)
                return;

            this._checkContentPosition = false;

            if (deltaY > 0 || deltaY < 0 && !this._scrollPosition) {
                this._inDrag = false;
                this._touched = false;
                return;
            }
        }

        this._touched = false;
        this._inDrag = true;
        var divider = this._windowHeight / 100;
        var deltaHeight = deltaY / divider;
        this._height += deltaHeight;
        this._currentDragPosition = position;
    }
}



const recursiveUpdate = (value, dataType, action) => {

    if (value == null) {
        value = document.querySelector(dataType);
        setTimeout(() => recursiveUpdate(value, dataType, action));
        return;
    }

    action(value);
}

function observeWatch(element, callback) {
    const handler = (entries) => {
        console.log('observeSticky:');
        console.log(entries[0]);
        // entries is an array of observed dom nodes
        // we're only interested in the first one at [0]
        // because that's our .sentinal node.
        // Here observe whether or not that node is in the viewport
        if (!entries[0].isIntersecting) {
            callback(true);
        } else {
            callback(false);
        }
    }

    // create the observer
    const observer = new window.IntersectionObserver(handler);
    // give the observer some dom nodes to keep an eye on
    observer.observe(element);
}

export function initializeBottomSheet (stops, passive, sensitivity, onClosedHandler) {
    return new BottomSheet(stops, passive, sensitivity, onClosedHandler);
}

export function openBottomSheet(bottomSheetInstance) {
    bottomSheetInstance.open();
}

export function closeBottomSheet(bottomSheetInstance) {
    bottomSheetInstance.close();
}