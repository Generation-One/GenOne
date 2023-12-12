class BottomSheet {
    /**
     *
     * @param {number[]} stops
     * @param {boolean} passive
     * @param {number} sensitivity
     * @param {function} onClosedHandler
     */
    constructor(stops = [], passive, sensitivity, onClosedHandler) {
        this._onClosedHandler = onClosedHandler;
        this._stops = stops;
        this._sensitivity = sensitivity;
        this._passive = passive;

        this.setupInitData();
        this.initializeSheet();
        this.reinitialize();
        this.addSheetObserve();
    }

    setupInitData() {
        this._hidden = true;
        this._height = 0;
        this._currentDragPosition = 0;
        this._startDragPosition = 0;
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
    }

    initializeSheet() {
        this._sheet = document.querySelector("[data-bs]");
        this._overlay = document.querySelector("[data-bs-overlay]");

        if (!this._sheet || !this._overlay) {
            throw new Error("[data-bs] or [data-bs-overlay] not found");
        }

        this._overlay.addEventListener("click", this.close);
        this._sheet.addEventListener("touchend", this.onDragEnd);
        this._sheet.addEventListener("mouseup", this.onDragEnd);
        this._sheet.addEventListener("touchmove", this.onDragTouch);
        this._sheet.addEventListener("mousemove", this.onDragMouse);
    }

    reinitialize() {
        this._header = document.querySelector("[data-bs-header]");
        this._content = document.querySelector("[data-bs-contents]");
        this._footer = document.querySelector("[data-bs-footer]");
        this._watch = document.querySelector("[data-bs-watch]");

        this._header?.addEventListener("touchstart", this.onDragStartTouch);
        this._header?.addEventListener("mousedown", this.onDragStartMouse);

        this._content?.addEventListener(
            "touchstart",
            this.onDragStartTouchContent
        );
        this._content?.addEventListener(
            "mousedown",
            this.onDragStartMouseContent
        );

        this._footer?.addEventListener("touchstart", this.onDragStartTouch);
        this._footer?.addEventListener("mousedown", this.onDragStartMouse);

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

        if (this._checkContentPosition) {
            if (deltaY == 0) return;

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

    onDragMouse = (event) => {
        this.onDragInternal(event.pageY);
        if (this._inDrag) {
            event.preventDefault();
            event.stopPropagation();
        }
    };

    onDragTouch = (event) => {
        this.onDragInternal(event.touches[0].clientY);
        if (this._inDrag) {
            event.preventDefault();
            event.stopPropagation();
        }
    };

    onDragStart(position, checkContentPosition) {
        if (this._passive) return;

        this._checkContentPosition = checkContentPosition;
        this._currentDragPosition = position;
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
        this.onDragStart(event.touches[0].clientY, false);
    };

    onDragStartMouse = (event) => {
        this.onDragStart(event.pageY, false);
    };

    onDragStartTouchContent = (event) => {
        if (!this._touched && !this._inDrag) {
            this.onDragStart(event.touches[0].clientY, true);
        }
    };

    onDragStartMouseContent = (event) => {
        if (!this._touched && !this._inDrag) {
            this.onDragStart(event.pageY, true);
        }
    };

    removeListeners = () => {
        this._header?.removeEventListener("touchstart", this.onDragStartTouch);
        this._header?.removeEventListener("mousedown", this.onDragStartMouse);

        this._content?.removeEventListener(
            "touchstart",
            this.onDragStartTouchContent
        );
        this._content?.removeEventListener(
            "mousedown",
            this.onDragStartMouseContent
        );

        this._footer?.removeEventListener("touchstart", this.onDragStartTouch);
        this._footer?.removeEventListener("mousedown", this.onDragStartMouse);
    };

    open() {
        if (!this._hidden) return;

        this._hidden = false;
        this._height = this._stops[0];

        const update = () => {
            if (this._hidden) this._sheet.setAttribute("aria-hidden", "");
            else this._sheet.removeAttribute("aria-hidden");

            this._contents = document.querySelector("[data-bs-contents]");
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

        if (this._hidden) {
            this._sheet.setAttribute("aria-hidden", "");
        } else {
            this._sheet.removeAttribute("aria-hidden");
        }
    }

    close = () => {
        if (this._hidden) return;

        this._hidden = true;
        this._height = 0;

        this._contents.addEventListener(
            "transitionend",
            () => {
                this._onClosedHandler.invokeMethodAsync("Callback");
            },
            { once: true }
        );

        requestAnimationFrame(() => {
            this.updateStyle();
        });
    };

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
 * @param {number[]} stops
 * @param {boolean} passive
 * @param {number} sensitivity
 * @param {function} onClosedHandler
 * @returns {BottomSheet}
 */
export const initializeBottomSheet = (
    stops,
    passive,
    sensitivity,
    onClosedHandler
) => {
    return new BottomSheet(stops, passive, sensitivity, onClosedHandler);
};

/**
 * Open bottom sheet
 * @param {BottomSheet} bottomSheetInstance
 */
export const openBottomSheet = (bottomSheetInstance) => {
    bottomSheetInstance.open();
};

/**
 * Close bottom sheet
 * @param {BottomSheet} bottomSheetInstance
 */
export const closeBottomSheet = (bottomSheetInstance) => {
    bottomSheetInstance.close();
};
