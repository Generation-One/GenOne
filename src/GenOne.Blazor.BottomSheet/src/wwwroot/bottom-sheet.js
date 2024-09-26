class BottomSheet {
    /**
     * @param {string} sheetId
     * @param {boolean} passive
     * @param {number} sensitivity
     * @param {function} onClosedHandler
     */
    constructor(sheetId, passive, sensitivity, onClosedHandler) {
        this._sheet = document.getElementById(sheetId);
        this._onClosedHandler = onClosedHandler;
        this._sensitivity = sensitivity;
        this._passive = passive;

        if (!this._sheet) {
            throw new Error("sheet not found");
        }

        this.setupInitData();
        this.initializeSheet();
        this.reinitialize();
        this.addSheetObserve();
    }

    setupInitData() {
        this._hidden = true;
        this._height = 0;
        this._currentDragPosition = 0;
        this._startDragPosition = {
            x: 0,
            y: 0
        };
        this._startDragHeight = 0;
        this._checkContentPosition = false;
        this._inDrag = false;
        this._dragFinished = false;
        this._touched = false;
        this._scrollPosition = true;
        this._sheetListenersAdded = false;
        this._windowHeight =
            window.innerHeight ||
            document.documentElement.clientHeight ||
            document.body.clientHeight;
        this._contents = this._sheet.querySelector("[data-bs-contents]");
    }

    initializeSheet() {
        this._overlay = this._sheet.querySelector("[data-bs-overlay]");

        if (!this._overlay) {
            throw new Error("[data-bs-overlay] not found");
        }

        this._overlay.addEventListener("click", this.close);
        this._sheet.addEventListener("touchend", this.onDragEnd);
        this._sheet.addEventListener("mouseup", this.onDragEnd);
        this._sheet.addEventListener("touchmove", this.onDragTouch);
        this._sheet.addEventListener("mousemove", this.onDragMouse);
    }

    reinitialize() {
        this._header = this._sheet.querySelector("[data-bs-header]");
        this._content = this._sheet.querySelector("[data-bs-contents]");
        this._footer = this._sheet.querySelector("[data-bs-footer]");
        this._watch = this._sheet.querySelector("[data-bs-watch]");

        if (this._header) {
            this._header.addEventListener("touchstart", this.onDragStartTouch);
            this._header.addEventListener("mousedown", this.onDragStartMouse);
        }

        if (this._content) {
            this._content.addEventListener(
                "touchstart",
                this.onDragStartTouchContent
            );
            this._content.addEventListener(
                "mousedown",
                this.onDragStartMouseContent
            );
        }

        if (this._footer) {
            this._footer.addEventListener("touchstart", this.onDragStartTouch);
            this._footer.addEventListener("mousedown", this.onDragStartMouse);
        }

        if (this._watch) {
            observeWatch(this._watch, (x) => (this._scrollPosition = !x));
        }
    }

    addSheetObserve() {
        observeMutation(this._sheet, () => {
            this.removeListeners();
            this.reinitialize();
        });
    }

    onDragInternal(position) {
        if (this._passive) return;

        if (!this._touched && !this._inDrag) return;

        const deltaY = this._currentDragPosition - position;

        if (deltaY == 0) return;

        if (this._checkContentPosition) {

            this._checkContentPosition = false;

            if (deltaY > 0 || (deltaY < 0 && !this._scrollPosition)) {
                this._inDrag = false;
                this._touched = false;
                return;
            }
        }

        this._touched = false;
        this._inDrag = true;

        const divider = this._windowHeight / 100;
        const deltaHeight = deltaY / divider;

        this._height += deltaHeight;
        this._currentDragPosition = position;
    }

    disableDrag = (position) => {
        const deltaX = position.x - this._startDragPosition.x;
        return Math.abs(deltaX) > Math.abs(position.y - this._startDragPosition.y)
    }

    onDragMouse = (event) => {
        const position = {
            y: event.pageY,
            x: event.pageX
        }
        if (!this.disableDrag(position)) {
            this.onDragInternal(event.pageY);
        }
        if (this._inDrag) {
            event.preventDefault();
            event.stopPropagation();
        }
    };

    onDragTouch = (event) => {
        const position = {
            y: event.touches[0].clientY,
            x: event.touches[0].clientX
        }
        if (!this.disableDrag(position)) {
            this.onDragInternal(event.touches[0].clientY);
        }
        if (this._inDrag) {
            event.preventDefault();
            event.stopPropagation();
        }
    };

    onDragStart(position, checkContentPosition) {
        if (this._passive) return;

        this._checkContentPosition = checkContentPosition;
        this._currentDragPosition = position.y;
        this._startDragPosition = position;
        this._startDragHeight = this._height;
        this._dragFinished = false;
        this._touched = true;

        const updateStylesInner = () => {
            this.updateStyle();

            if (!this._dragFinished) {
                requestAnimationFrame(updateStylesInner);
            }
        };

        requestAnimationFrame(updateStylesInner);
    }

    onDragEnd = () => {
        this._currentDragPosition = 0;
        this._dragFinished = true;
        this._touched = false;
        this._inDrag = false;

        if (this._passive) return;

        if (this._height < this._stops[0] - this._sensitivity) {
            this.close();
        } else {
            this._height = this.getClosestStop(this._height);
        }

        this._sheet.removeAttribute("data-bs-scroll");
    };

    onDragStartTouch = (event) => {
        const position = {
            y: event.touches[0].clientY,
            x: event.touches[0].clientX
        }

        this.onDragStart(posiiton, false);
    };

    onDragStartMouse = (event) => {
        const position = {
            y: event.pageY,
            x: event.pageX
        }

        this.onDragStart(position, false);
    };

    onDragStartTouchContent = (event) => {
        if (!this._touched && !this._inDrag) {
            const position = {
                y: event.touches[0].clientY,
                x: event.touches[0].clientX
            }
            this.onDragStart(position, true);
        }
    };

    onDragStartMouseContent = (event) => {
        if (!this._touched && !this._inDrag) {
            const position = {
                y: event.pageY,
                x: event.pageX
            }
            this.onDragStart(position, true);
        }
    };

    removeListeners = () => {

        if (this._header) {
            this._header.removeEventListener("touchstart", this.onDragStartTouch);
            this._header.removeEventListener("mousedown", this.onDragStartMouse);
        }

        if (this._content) {
            this._content.removeEventListener(
                "touchstart",
                this.onDragStartTouchContent
            );
            this._content.removeEventListener(
                "mousedown",
                this.onDragStartMouseContent
            );
        }

        if (this._footer) {
            this._footer.removeEventListener("touchstart", this.onDragStartTouch);
            this._footer.removeEventListener("mousedown", this.onDragStartMouse);
        }
    };

    /**
     * Open bottom sheet
     * @param {number[]} stops
     */
    open(stops) {
        if (!this._hidden) return;

        this._stops = stops;

        this._hidden = false;
        this._height = this._stops[0];

        const update = () => {
            this.updateAriaHidden();
            this._contents.style.height = this._height + "vh";
        };

        requestAnimationFrame(update);
    }

    updateStyle() {
        if (this._inDrag) {
            this._sheet.setAttribute("data-bs-scroll", "");
        } else {
            this._sheet.removeAttribute("data-bs-scroll");
        }

        this._contents.style.height = this._height + "vh";

        if (this._height === 100) {
            this._contents.setAttribute("full-screen", "");
        } else {
            this._contents.removeAttribute("full-screen");
        }

        this.updateAriaHidden();
    }

    close = () => {
        if (this._hidden) return;

        this._hidden = true;
        this._height = 0;

        this._contents.addEventListener(
            "transitionend",
            () => {
                this._onClosedHandler?.invokeMethodAsync("Callback");
            }, {
                once: true
            }
        );

        requestAnimationFrame(() => {
            this.updateStyle();
        });
    };

    updateAriaHidden() {
        if (this._hidden) {
            this._sheet.setAttribute("aria-hidden", "");
        } else {
            this._sheet.removeAttribute("aria-hidden");
        }
    }

    getClosestStop(goal) {
        if (this._stops.length == 1) return this._stops[0];

        return this._stops.reduce((prev, cur) =>
            Math.abs(cur - goal) < Math.abs(prev - goal) ? cur : prev
        );
    }
}

const observeMutation = (element, callback) => {
    const observer = new MutationObserver(callback);
    observer.observe(element, {
        childList: true,
        subtree: true,
    });
};

const observeWatch = (element, callback) => {
    const handler = (entries) => {
        if (!entries[0].isIntersecting) {
            callback(true);
        } else {
            callback(false);
        }
    };

    const observer = new window.IntersectionObserver(handler);
    observer.observe(element);
};

/**
 * Initialize bottom sheet
 * @param {string} sheet
 * @param {boolean} passive
 * @param {number} sensitivity
 * @param {function} onClosedHandler
 * @returns {BottomSheet}
 */
export const initializeBottomSheet = (
    sheetId,
    passive,
    sensitivity,
    onClosedHandler
) => {
    return new BottomSheet(sheetId, passive, sensitivity, onClosedHandler);
};

/**
 * Close bottom sheet
 * @param {string} sheetId
 * @param {string} bottomOffset
 */
export const addBottomOffset = (sheetId, bottomOffset) => {
    document.getElementById(sheetId).querySelector('[data-bs-contents]').style.bottom = bottomOffset;
};
