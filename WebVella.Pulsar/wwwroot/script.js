﻿window.WebVellaPulsar = {
    // Variables in alpha sort
    elementCheckIntervalRetries: 10,
    cKEditors: {},
    flatPickrs: {},
    eventListeners: {},
    outsideClickListeners: {},
    infiniteScrollObservers: {},
    observedItemObservers: {},
    observedItemObserverTimouts: {},
    dispose: function () {
        if (WebVellaPulsar.cKEditors) {
            for (var elementId in WebVellaPulsar.cKEditors) {
                WebVellaPulsar.cKEditors[elementId].destroy();
                delete WebVellaPulsar.cKEditors[elementId];
            }
        }
        if (WebVellaPulsar.flatPickrs) {
            for (var elementId in WebVellaPulsar.flatPickrs) {
                WebVellaPulsar.flatPickrs[elementId].destroy();
                delete WebVellaPulsar.flatPickrs[elementId];
            }
        }
        if (WebVellaPulsar.eventListeners) {
            for (var eventName in WebVellaPulsar.eventListeners) {
                if (WebVellaPulsar.eventListeners[eventName]) {
                    for (var listenerId in WebVellaPulsar.eventListeners[eventName]) {
                        WebVellaPulsar.removeDocumentEventListener(eventName, listenerId);
                    }
                }
            }
        }
        if (WebVellaPulsar.outsideClickListeners) {
            for (var elementSelector in WebVellaPulsar.outsideClickListeners) {
                if (WebVellaPulsar.outsideClickListeners[elementSelector]) {
                    for (var listenerId in WebVellaPulsar.outsideClickListeners[elementSelector]) {
                        WebVellaPulsar.removeOutsideClickEventListener(elementSelector, listenerId);
                    }
                }
            }
        }
        if (WebVellaPulsar.infiniteScrollObservers) {
            for (var componentId in WebVellaPulsar.infiniteScrollObservers) {
                WebVellaPulsar.infiniteScrollDestroy(componentId);
            }
        }
        if (WebVellaPulsar.observedItemObservers) {
            for (var componentId in WebVellaPulsar.observedItemObservers) {
                WebVellaPulsar.observedItemDestroy(componentId);
            }
        }
        return true;
    },
    // Functions in alpha sort
    addBackdrop: function () {
        var backdropEl = document.querySelector(".modal-backdrop");
        if (!backdropEl) {
            const bdEl = document.createElement('div');
            bdEl.classList.add("modal-backdrop");
            bdEl.classList.add("show");
            document.body.appendChild(bdEl);
        }
        return true;
    },
    addBodyClass: function (Classname) {
        if (Classname === "modal-open") {
            WebVellaPulsar.changeBodyPaddingRight("17px");
            WebVellaPulsar.addBackdrop();
        }
        document.body.classList.add(Classname);
        return true;
    },
    addCKEditor: function (elementId, dotNetHelper, cultureString) {
        if (!WebVellaPulsar.cKEditors[elementId]) {
            var config = {
                attributes: {
                    class: 'form-control'
                }
            };
            if (cultureString) {
                config.language = cultureString;
            }
            else {
                config.language = 'en';
            }

            var retries = 0;
            var initInterval = setInterval(function () {
                if (document.getElementById(elementId)) {
                    clearInterval(initInterval);
                    WebVellaPulsar.initCKEditor(elementId, dotNetHelper, config);
                }
                if (retries > WebVellaPulsar.elementCheckIntervalRetries) {
                    clearInterval(initInterval);
                }
                retries++;
            }, 10);
        }
        return true;
    },
    addDocumentEventListener: function (eventName, dotNetHelper, listenerId, methodName) {
        if (!WebVellaPulsar.eventListeners[eventName]) {
            WebVellaPulsar.eventListeners[eventName] = {};
        }
        WebVellaPulsar.eventListeners[eventName][listenerId] = { dotNetHelper: dotNetHelper, methodName: methodName };
        return true;
    },
    addFlatPickrDate: function (elementId, dotNetHelper, cultureString) {
        if (!WebVellaPulsar.flatPickrs[elementId]) {
            if (!cultureString) {
                cultureString = 'en';
            }
            var retries = 0;
            var initInterval = setInterval(function () {
                if (document.getElementById(elementId)) {
                    clearInterval(initInterval);
                    WebVellaPulsar.initFlatPickrDate(elementId, dotNetHelper, cultureString);
                }
                if (retries > WebVellaPulsar.elementCheckIntervalRetries) {
                    clearInterval(initInterval);
                }
                retries++;
            }, 10);
        }
        return true;
    },
    addFlatPickrDateTime: function (elementId, dotNetHelper, utcOffsetInSeconds, cultureString) {
        if (!WebVellaPulsar.flatPickrs[elementId]) {
            if (!cultureString) {
                cultureString = 'en';
            }
            var retries = 0;
            var initInterval = setInterval(function () {
                if (document.getElementById(elementId)) {
                    clearInterval(initInterval);
                    WebVellaPulsar.initFlatPickrDateTime(elementId, dotNetHelper, utcOffsetInSeconds, cultureString);
                }
                if (retries > WebVellaPulsar.elementCheckIntervalRetries) {
                    clearInterval(initInterval);
                }
                retries++;
            }, 10);
        }
        return true;
    },
    addOutsideClickEventListener: function (elementSelector, dotNetHelper, listenerId, methodName) {
        if (!WebVellaPulsar.outsideClickListeners[elementSelector]) {
            WebVellaPulsar.outsideClickListeners[elementSelector] = {};
        }
        WebVellaPulsar.outsideClickListeners[elementSelector][listenerId] = { dotNetHelper: dotNetHelper, methodName: methodName };
        return true;
    },
    appStart: function () {
        document.getElementById("wvp-blazor-loader").remove();
        return true;
    },
    updateAppStartProgress: function (progress) {
        var progressEl = document.getElementById("wvp-blazor-loader-progress");
        progressEl.innerHTML = progress;
        return true;
    },
    blurElement: function (elementId) {
        window.setTimeout(function () {
            const element = document.getElementById(elementId);
            if (element) {
                element.blur();
            }
            return true;
        }, 100);
        return true;
    },
    blurElementBySelector: function (elementSelector) {
        window.setTimeout(function () {
            const element = document.querySelector(elementSelector);
            if (element) {
                element.blur();
            }
            return true;
        }, 100);
        return true;
    },
    changeBodyPaddingRight: function (padding) {
        var dpi = window.devicePixelRatio;
        if (dpi === 1 || !padding) {
            document.body.style.paddingRight = padding;
        }
        return true;
    },
    checkIfElementVisibleAsync: function (domElement) {
        if (domElement) {
            return new Promise(resolve => {
                const o = new IntersectionObserver(([entry]) => {
                    resolve(entry.intersectionRatio > 0);
                    o.disconnect();
                });
                o.observe(domElement);
            });
        }
        return false;
    },
    checkIfElementIdVisible: async function (elementId) {
        var element = document.getElementById(elementId);
        if (element) {
            var result = await WebVellaPulsar.checkIfElementVisibleAsync(element);
            return result;
        }
        return false;
    },
    clearFlatPickrDate: function (elementId) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            WebVellaPulsar.flatPickrs[elementId].clear();
            WebVellaPulsar.flatPickrs[elementId].close();
        }
        return true;
    },
    clearFlatPickrDateTime: function (elementId) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            WebVellaPulsar.flatPickrs[elementId].clear();
            WebVellaPulsar.flatPickrs[elementId].close();
        }
        return true;
    },
    makeDraggable: function (elementId) {
        var element = document.getElementById(elementId);
        if (!element)
            return;
        var handleEl = element.querySelector(".drag-handle");
        if (!handleEl)
            handleEl = element.querySelector(".modal-header");

        if (!handleEl)
            handleEl = element.querySelector(".modal-body");

        if (!handleEl)
            return;

        var isMouseDown = false;

        // initial mouse X and Y for `mousedown`
        var mouseX;
        var mouseY;

        // element X and Y before and after move
        var elementX = 0;
        var elementY = 0;

        // mouse button down over the element
        handleEl.addEventListener('mousedown', onMouseDown);

        /**
         * Listens to `mousedown` event.
         *
         * @param {Object} event - The event.
         */
        function onMouseDown(event) {
            mouseX = event.clientX;
            mouseY = event.clientY;
            isMouseDown = true;
        }

        // mouse button released
        handleEl.addEventListener('mouseup', onMouseUp);

        /**
         * Listens to `mouseup` event.
         *
         * @param {Object} event - The event.
         */
        function onMouseUp(event) {
            isMouseDown = false;
            elementX = parseInt(element.style.left) || 0;
            elementY = parseInt(element.style.top) || 0;
        }

        // need to attach to the entire document
        // in order to take full width and height
        // this ensures the element keeps up with the mouse
        document.addEventListener('mousemove', onMouseMove);


        /**
         * Listens to `mousemove` event.
         *
         * @param {Object} event - The event.
         */
        function onMouseMove(event) {
            if (!isMouseDown) return;
            var deltaX = event.clientX - mouseX;
            var deltaY = event.clientY - mouseY;
            element.style.left = elementX + deltaX + 'px';
            element.style.top = elementY + deltaY + 'px';
        }
        return true;
    },
    removeDraggable: function (elementId) {
        return true;
    },
    executeEventCallbacks: function (eventName, e) {
        if (WebVellaPulsar.eventListeners[eventName]) {
            for (const prop in WebVellaPulsar.eventListeners[eventName]) {
                const dotNetHelper = WebVellaPulsar.eventListeners[eventName][prop].dotNetHelper;
                const methodName = WebVellaPulsar.eventListeners[eventName][prop].methodName;
                if (dotNetHelper && methodName) {
                    if (eventName === "keydown") {
                        dotNetHelper.invokeMethodAsync(methodName, WebVellaPulsar.serializeKeyDownEvent(e))
                    }
                    else {
                        dotNetHelper.invokeMethodAsync(methodName)
                    }
                }
            }
        }
        return true;
    },
    executeOutsideClickEventCallbacks: function (elementSelector, context) {
        if (WebVellaPulsar.outsideClickListeners[elementSelector]) {
            for (const prop in WebVellaPulsar.outsideClickListeners[elementSelector]) {
                const dotNetHelper = WebVellaPulsar.outsideClickListeners[elementSelector][prop].dotNetHelper;
                const methodName = WebVellaPulsar.outsideClickListeners[elementSelector][prop].methodName;
                if (dotNetHelper && methodName) {
                    dotNetHelper.invokeMethodAsync(methodName)
                }
            }
        }
        return true;
    },
    focusElement: function (elementId) {
        window.setTimeout(function () {
            const element = document.getElementById(elementId);
            if (element) {
                element.focus();
            }
            return true;
        }, 100);
        return true;
    },
    focusElementBySelector: function (elementSelector) {
        window.setTimeout(function () {
            const element = document.querySelector(elementSelector);
            if (element) {
                element.focus();
            }
            return true;
        }, 100);
        return true;
    },
    getArrayBufferFromFileAsync: function (elem, fileId) {
        var file = WebVellaPulsar.getFileById(elem, fileId);

        // On the first read, convert the FileReader into a Promise<ArrayBuffer>
        if (!file.readPromise) {
            file.readPromise = new Promise(function (resolve, reject) {
                var reader = new FileReader();
                reader.onload = function () { resolve(reader.result); };
                reader.onerror = function (err) { reject(err); };
                reader.readAsArrayBuffer(file.blob);
            });
        }

        return file.readPromise;
    },
    getFileById: function (elem, fileId) {
        var file = elem.filesById[fileId];
        if (!file) {
            throw new Error('There is no file with ID ' + fileId + '. The file list may have changed');
        }

        return file;
    },
    getSelectedValues: function (sel) {
        var results = [];
        if (sel) {
            var i;
            for (i = 0; i < sel.options.length; i++) {
                if (sel.options[i].selected) {
                    results[results.length] = sel.options[i].value;
                }
            }
        }
        return results;
    },
    getTimezoneOffset: function () {
        return new Date().getTimezoneOffset();
    },
    initCKEditor: function (elementId, dotNetHelper, config) {
        ClassicEditor
            .create(document.getElementById(elementId), config)
            .then(function (editor) {
                WebVellaPulsar.cKEditors[elementId] = editor;
                editor.ui.focusTracker.on('change:isFocused', function () {
                    var value = editor.getData();
                    document.getElementById(elementId).value = value;
                    const e = new Event("change");
                    document.getElementById(elementId).dispatchEvent(e);
                    dotNetHelper.invokeMethodAsync("NotifyChange", value).then(function () {
                    }, function (err) {
                        throw new Error(err);
                    });
                });

            })
            .catch(function (error) {
                console.error(error);
            });
    },
    initFlatPickrDate: function (elementId, dotNetHelper, cultureString) {
        var selector = "#" + elementId;
        var flatPickrServerDateTimeFormat = "Y-m-d";
        //From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
        var flatPickrUiDateTimeFormat = "d M Y";
        var BulgarianDateTimeLocale = {
            firstDayOfWeek: 1,
            weekdays: {
                shorthand: ["Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                longhand: [
                    "Неделя",
                    "Понеделник",
                    "Вторник",
                    "Сряда",
                    "Четвъртък",
                    "Петък",
                    "Събота"
                ]
            },

            months: {
                shorthand: [
                    "яну",
                    "фев",
                    "март",
                    "апр",
                    "май",
                    "юни",
                    "юли",
                    "авг",
                    "сеп",
                    "окт",
                    "ное",
                    "дек"
                ],
                longhand: [
                    "Януари",
                    "Февруари",
                    "Март",
                    "Април",
                    "Май",
                    "Юни",
                    "Юли",
                    "Август",
                    "Септември",
                    "Октомври",
                    "Ноември",
                    "Декември"
                ]
            }
        };
        var fp = document.querySelector(selector)._flatpickr;
        if (!fp) {
            var options = {
                time_24hr: true,
                dateFormat: flatPickrServerDateTimeFormat,
                defaultDate: null,
                locale: cultureString,
                enableTime: false,
                static: false,
                minuteIncrement: 1,
                altInput: true,
                allowInput: true,
                altFormat: flatPickrUiDateTimeFormat,
                parseDate: (datestr, format) => {

                    return moment(datestr).toDate();
                    //return moment(datestr, format, true).toDate();
                },
                onChange: function (selectedDates, dateStr, instance) {
                    //Convert to date without kind
                    var selectedDate = null;
                    if (selectedDates.length > 0 && selectedDates[0]) {
                        selectedDate = moment(selectedDates[0]).format('YYYY-MM-DD');
                    }
                    dotNetHelper.invokeMethodAsync("NotifyChange", selectedDate);
                },
            };
            if (cultureString && cultureString === "bg") {
                options.locale = BulgarianDateTimeLocale;
            }
            WebVellaPulsar.flatPickrs[elementId] = flatpickr(selector, options);
            WebVellaPulsar.flatPickrs[elementId].altInput.addEventListener("blur", function (e) {
                if (!e.target.value) {
                    WebVellaPulsar.flatPickrs[elementId].clear();
                }
            });

        }
    },
    initFlatPickrDateTime: function (elementId, dotNetHelper, cultureString) {
        var selector = "#" + elementId;
        var flatPickrServerDateTimeFormat = "Y-m-dTH:i:S";//Z
        //From the server dates will be received yyyy-MM-ddTHH:mm:ss.fff
        var flatPickrUiDateTimeFormat = "d M Y H:i";
        var BulgarianDateTimeLocale = {
            firstDayOfWeek: 1,
            weekdays: {
                shorthand: ["Нд", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                longhand: [
                    "Неделя",
                    "Понеделник",
                    "Вторник",
                    "Сряда",
                    "Четвъртък",
                    "Петък",
                    "Събота"
                ]
            },

            months: {
                shorthand: [
                    "яну",
                    "фев",
                    "март",
                    "апр",
                    "май",
                    "юни",
                    "юли",
                    "авг",
                    "сеп",
                    "окт",
                    "ное",
                    "дек"
                ],
                longhand: [
                    "Януари",
                    "Февруари",
                    "Март",
                    "Април",
                    "Май",
                    "Юни",
                    "Юли",
                    "Август",
                    "Септември",
                    "Октомври",
                    "Ноември",
                    "Декември"
                ]
            }
        };
        var fp = document.querySelector(selector)._flatpickr;
        if (!fp) {
            var options = {
                time_24hr: true,
                dateFormat: flatPickrServerDateTimeFormat,
                defaultDate: null,
                locale: cultureString,
                enableTime: true,
                static: false,
                minuteIncrement: 1,
                altInput: true,
                allowInput: true,
                altFormat: flatPickrUiDateTimeFormat,
                onChange: function (selectedDates, dateStr, instance) {
                    var selectedDate = null;
                    if (selectedDates.length > 0 && selectedDates[0]) {
                        selectedDate = moment(selectedDates[0]).format("YYYY-MM-DDTHH:mm:SS");
                    }
                    dotNetHelper.invokeMethodAsync("NotifyChange", selectedDate);
                },
            };
            if (cultureString && cultureString === "bg") {
                options.locale = BulgarianDateTimeLocale;
            }
            WebVellaPulsar.flatPickrs[elementId] = flatpickr(selector, options);
            WebVellaPulsar.flatPickrs[elementId].altInput.addEventListener("blur", function (e) {
                if (!e.target.value) {
                    WebVellaPulsar.flatPickrs[elementId].clear();
                }
            });

        }
    },
    initInfiniteScroll: function (componentId, dotNetHelper, observerTargetId, observerViewportId) {
        var options = {
            root: null,
            threshold: 0
        };
        if (observerViewportId) {
            options.root = document.getElementById(observerViewportId);
        };
        WebVellaPulsar.infiniteScrollObservers[componentId] = new IntersectionObserver(
            function (e) {
                dotNetHelper.invokeMethodAsync("OnIntersection");
            },
            options
        );

        let element = document.getElementById(observerTargetId);
        if (element !== null) {
            WebVellaPulsar.infiniteScrollObservers[componentId].observe(element);
        }
        return true;
    },
    infiniteScrollDestroy: function (componentId) {
        if (WebVellaPulsar.infiniteScrollObservers[componentId]) {
            try {
                WebVellaPulsar.infiniteScrollObservers[componentId].disconnect();
                delete WebVellaPulsar.infiniteScrollObservers[componentId];
            } catch (e) { }
        }
        return true;
    },
    initUploadFile: function (elemId, dotNetHelper) {
        var elem = document.getElementById(elemId);

        elem.addEventListener('change', function handleInputFileChange(event) {
            elem.filesById = {};
            var fileList = Array.prototype.map.call(elem.files, function (file) {
                var result = {
                    id: Date.now(),
                    lastModified: new Date(file.lastModified).toISOString(),
                    name: file.name,
                    size: file.size,
                    contentType: file.type
                };
                elem.filesById[result.id] = result;

                // Attach the blob data itself as a non-enumerable property so it doesn't appear in the JSON
                Object.defineProperty(result, 'blob', { value: file });

                return result;
            });

            dotNetHelper.invokeMethodAsync("NotifyChange", fileList).then(function () {
                elem.value = '';
            }, function (err) {
                elem.value = '';
                throw new Error(err);
            });
        });

        return true;
    },
    observedItemInit: function (componentId, dotNetHelper, observerTargetId, observerViewportId) {
        var options = {
            root: null,
            threshold: 0,
            delay:0
        };
        if (observerViewportId) {
            options.root = document.getElementById(observerViewportId);
        };
        //if (WebVellaPulsar.observedItemObservers[componentId])
        //	return;
        WebVellaPulsar.observedItemObservers[componentId] = new IntersectionObserver(
            function (e, ob) {
                if (e && e.length > 0 && e[0].isIntersecting) {
                    dotNetHelper.invokeMethodAsync("OnIntersection");
                }
            },
            options
        );

        let element = document.getElementById(observerTargetId);
        if (element !== null) {
            WebVellaPulsar.observedItemObservers[componentId].observe(element);
        }
        return true;
    },
    observedItemDestroy: function (componentId) {
        if (WebVellaPulsar.observedItemObservers[componentId]) {
            try {
                WebVellaPulsar.observedItemObservers[componentId].disconnect();
                delete WebVellaPulsar.observedItemObservers[componentId];
            } catch (e) { }
        }
        return true;
    },
    readFileData: function (elem, fileId, startOffset, count) {
        var readPromise = WebVellaPulsar.getArrayBufferFromFileAsync(elem, fileId);

        return readPromise.then(function (arrayBuffer) {
            var uint8Array = new Uint8Array(arrayBuffer, startOffset, count);
            var base64 = WebVellaPulsar.uint8ToBase64(uint8Array);
            return base64;
        })
    },
    reloadPage: function () {
        window.location.reload();
        return true;
    },
    removeBackdrop: function () {
        const bdEl = document.querySelector(".modal-backdrop");
        if (bdEl) {
            bdEl.remove();
        }
        return true;
    },
    removeBodyClass: function (Classname) {
        if (Classname === "modal-open") {
            WebVellaPulsar.changeBodyPaddingRight("");
            WebVellaPulsar.removeBackdrop();
        }
        document.body.classList.remove(Classname);
        return true;
    },
    getModalCount: function () {
        var currentModalCountString = document.body.dataset.modalCount;
        if (!currentModalCountString) {
            currentModalCountString = "0";
        }
        var result = 0;
        try {
            result = parseInt(currentModalCountString);
        }
        catch {
            return 0;
        }

        return result;
    },
    addModalCount: function () {
        var currentModalCount = WebVellaPulsar.getModalCount();
        var newModalCount = currentModalCount + 1;
        document.body.dataset.modalCount = newModalCount;
        return newModalCount;
    },
    removeModalCount: function () {
        var currentModalCount = WebVellaPulsar.getModalCount();
        var newModalCount = currentModalCount;
        if (newModalCount > 0) {
            newModalCount = newModalCount - 1;
            document.body.dataset.modalCount = newModalCount;
        }
        return newModalCount;
    },
    setModalOpen: function () {
        var newModalCount = WebVellaPulsar.addModalCount();
        if (newModalCount == 1)
            WebVellaPulsar.addBodyClass("modal-open");

        return true;
    },
    setModalClose: function () {
        var newModalCount = WebVellaPulsar.removeModalCount();
        if (newModalCount == 0)
            WebVellaPulsar.removeBodyClass("modal-open");
        return true;
    },
    setCKEditorData: function (elementId, data) {
        if (WebVellaPulsar.cKEditors[elementId]) {
            WebVellaPulsar.cKEditors[elementId].setData(data);
        }
        return true;
    },
    removeCKEditor: function (elementId) {
        if (WebVellaPulsar.cKEditors[elementId]) {
            WebVellaPulsar.cKEditors[elementId].destroy();
            delete WebVellaPulsar.cKEditors[elementId];
        }
        return true;
    },
    removeDocumentEventListener: function (eventName, listenerId) {
        if (WebVellaPulsar.eventListeners[eventName] && WebVellaPulsar.eventListeners[eventName][listenerId]) {
            delete WebVellaPulsar.eventListeners[eventName][listenerId];
        }
        return true;
    },
    removeFlatPickrDateTime: function (elementId) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            WebVellaPulsar.flatPickrs[elementId].destroy();
            delete WebVellaPulsar.flatPickrs[elementId];
        }
        return true;
    },
    removeFlatPickrDate: function (elementId) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            WebVellaPulsar.flatPickrs[elementId].destroy();
            delete WebVellaPulsar.flatPickrs[elementId];
        }
        return true;
    },
    removeOutsideClickEventListener: function (elementSelector, listenerId) {
        if (WebVellaPulsar.outsideClickListeners[elementSelector] && WebVellaPulsar.outsideClickListeners[elementSelector][listenerId]) {
            delete WebVellaPulsar.outsideClickListeners[elementSelector][listenerId];
        }
        return true;
    },
    scrollElement: function (el, x, y) {
        el.scroll(x, y);
        return true;
    },
    setElementHtml: function (elementId, html) {
        var el = document.getElementById(elementId);
        if (el) {
            el.innerHTML = html;
        }
        return true;
    },
    setPageMetaTitle: function (title) {
        document.title = title;
        return true;
    },
    setFlatPickrDateChange: function (elementId, dateString) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            //Should not notify the change as it will loop
            WebVellaPulsar.flatPickrs[elementId].setDate(dateString, false, "Y-m-d");
        }
        return true;
    },
    setFlatPickrDateTimeChange: function (elementId, dateString) {
        if (WebVellaPulsar.flatPickrs[elementId]) {
            //Should not notify the change as it will loop
            WebVellaPulsar.flatPickrs[elementId].setDate(dateString, false, "Y-m-dTH:i:S");
        }
        return true;
    },
    simulateClick: function (el) {
        el.click();
        return true;
    },
    simulateClickById: function (elementId) {
        var el = document.getElementById(elementId);
        if (el) {
            el.click();
        }
        return true;
    },
    showToast: function (title, message, type, duration) {
        var options = {
            text: "<div class='toastify__meta'><div class='toastify__meta__title'>" + title + "</div><div  class='toastify__meta__content'>" + message + "</div></div>",
            duration: duration,
            newWindow: true,
            close: true,
            gravity: "top", // `top` or `bottom`
            position: 'center', // `left`, `center` or `right`
            className: "toastify--" + type,
            stopOnFocus: true, // Prevents dismissing of toast on hover
            onClick: function () { } // Callback after click
        }

        Toastify(options).showToast();
        return true;
    },
    uint8ToBase64: (function () {
        // Code from https://github.com/beatgammit/base64-js/
        // License: MIT
        var lookup = [];

        var code = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';
        for (var i = 0, len = code.length; i < len; ++i) {
            lookup[i] = code[i];
        }

        function tripletToBase64(num) {
            return lookup[num >> 18 & 0x3F] +
                lookup[num >> 12 & 0x3F] +
                lookup[num >> 6 & 0x3F] +
                lookup[num & 0x3F];
        }

        function encodeChunk(uint8, start, end) {
            var tmp;
            var output = [];
            for (var i = start; i < end; i += 3) {
                tmp =
                    ((uint8[i] << 16) & 0xFF0000) +
                    ((uint8[i + 1] << 8) & 0xFF00) +
                    (uint8[i + 2] & 0xFF);
                output.push(tripletToBase64(tmp));
            }
            return output.join('');
        }

        return function fromByteArray(uint8) {
            var tmp;
            var len = uint8.length;
            var extraBytes = len % 3; // if we have 1 byte left, pad 2 bytes
            var parts = [];
            var maxChunkLength = 16383; // must be multiple of 3

            // go through the array every three bytes, we'll deal with trailing stuff later
            for (var i = 0, len2 = len - extraBytes; i < len2; i += maxChunkLength) {
                parts.push(encodeChunk(
                    uint8, i, (i + maxChunkLength) > len2 ? len2 : (i + maxChunkLength)
                ));
            }

            // pad the end with zeros, but make sure to not forget the extra bytes
            if (extraBytes === 1) {
                tmp = uint8[len - 1];
                parts.push(
                    lookup[tmp >> 2] +
                    lookup[(tmp << 4) & 0x3F] +
                    '=='
                );
            } else if (extraBytes === 2) {
                tmp = (uint8[len - 2] << 8) + uint8[len - 1];
                parts.push(
                    lookup[tmp >> 10] +
                    lookup[(tmp >> 4) & 0x3F] +
                    lookup[(tmp << 2) & 0x3F] +
                    '='
                );
            }

            return parts.join('');
        };
    })(),
    scrollToElement: function (elementId) {
        var elmnt = document.getElementById(elementId);
        if (elmnt)
            elmnt.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });

        return true;
    },
    serializeKeyDownEvent: function (e) {
        if (e) {
            var o = {
                altKey: e.altKey,
                ctrlKey: e.ctrlKey,
                metaKey: e.metaKey,
                shiftKey: e.shiftKey,
                code: e.code
            };
            return o;
        }
    },
    logError: function (message) {
        console.error(message);
        return true;
    },
    logInfo: function (message) {
        console.info(message);
        return true;
    }
};

//Listeners
document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        WebVellaPulsar.executeEventCallbacks("keydown-escape", e);
    }
    else if (e.key === "Enter") {
        WebVellaPulsar.executeEventCallbacks("keydown-enter", e);
    }

    WebVellaPulsar.executeEventCallbacks("keydown", e);
});
document.addEventListener("mousedown", function (e) {
    //Check if not clicked on scrollbars
    //If this is a standard scrollable body this should work
    if (window.outerWidth <= e.clientX) {
        return true;
    }
    //If one of the elements has scroll
    if (e.target.classList.contains("scrollable-wrapper") && e.target.clientWidth <= e.clientX)
        return true;


    WebVellaPulsar.executeEventCallbacks("mousedown", e);
    if (!e.target.closest('.dropdown-menu')) {
        WebVellaPulsar.executeEventCallbacks("mousedown-non-dropdown", e);
    }
    if (!e.target.closest('.modal')) {
        WebVellaPulsar.executeEventCallbacks("mousedown-non-modal", e);
    }

    if (WebVellaPulsar.outsideClickListeners) {
        var hasOutSideListeners = false;
        for (var elementSelector in WebVellaPulsar.outsideClickListeners) {
            if (WebVellaPulsar.outsideClickListeners.hasOwnProperty(elementSelector)) {
                if (!e.target.closest(elementSelector) && !e.target.closest(".ck-editor")) {
                    WebVellaPulsar.executeOutsideClickEventCallbacks(elementSelector, e);
                }
            }
        }
    }
});


/*!
 * Toastify js 1.8.0
 * https://github.com/apvarun/toastify-js
 * @license MIT licensed
 *
 * Copyright (C) 2018 Varun A P
 */
(function (root, factory) {
    if (typeof module === "object" && module.exports) {
        module.exports = factory();
    } else {
        root.Toastify = factory();
    }
})(this, function (global) {
    // Object initialization
    var Toastify = function (options) {
        // Returning a new init object
        return new Toastify.lib.init(options);
    },
        // Library version
        version = "1.8.0";

    // Defining the prototype of the object
    Toastify.lib = Toastify.prototype = {
        toastify: version,

        constructor: Toastify,

        // Initializing the object with required parameters
        init: function (options) {
            // Verifying and validating the input object
            if (!options) {
                options = {};
            }

            // Creating the options object
            this.options = {};

            this.toastElement = null;

            // Validating the options
            this.options.text = options.text || "Hi there!"; // Display message
            this.options.node = options.node // Display content as node
            this.options.duration = options.duration === 0 ? 0 : options.duration || 3000; // Display duration
            this.options.selector = options.selector; // Parent selector
            this.options.callback = options.callback || function () { }; // Callback after display
            this.options.destination = options.destination; // On-click destination
            this.options.newWindow = options.newWindow || false; // Open destination in new window
            this.options.close = options.close || false; // Show toast close icon
            this.options.gravity = options.gravity === "bottom" ? "toastify-bottom" : "toastify-top"; // toast position - top or bottom
            this.options.positionLeft = options.positionLeft || false; // toast position - left or right
            this.options.position = options.position || ''; // toast position - left or right
            this.options.backgroundColor = options.backgroundColor; // toast background color
            this.options.avatar = options.avatar || ""; // img element src - url or a path
            this.options.className = options.className || ""; // additional class names for the toast
            this.options.stopOnFocus = options.stopOnFocus === undefined ? true : options.stopOnFocus; // stop timeout on focus
            this.options.onClick = options.onClick; // Callback after click

            // Returning the current object for chaining functions
            return this;
        },

        // Building the DOM element
        buildToast: function () {
            // Validating if the options are defined
            if (!this.options) {
                throw "Toastify is not initialized";
            }

            // Creating the DOM object
            var divElement = document.createElement("div");
            divElement.className = "toastify on " + this.options.className;

            // Positioning toast to left or right or center
            if (!!this.options.position) {
                divElement.className += " toastify-" + this.options.position;
            } else {
                // To be depreciated in further versions
                if (this.options.positionLeft === true) {
                    divElement.className += " toastify-left";
                    console.warn('Property `positionLeft` will be depreciated in further versions. Please use `position` instead.')
                } else {
                    // Default position
                    divElement.className += " toastify-right";
                }
            }

            // Assigning gravity of element
            divElement.className += " " + this.options.gravity;

            if (this.options.backgroundColor) {
                divElement.style.background = this.options.backgroundColor;
            }

            // Adding the toast message/node
            if (this.options.node && this.options.node.nodeType === Node.ELEMENT_NODE) {
                // If we have a valid node, we insert it
                divElement.appendChild(this.options.node)
            } else {
                divElement.innerHTML = this.options.text;

                if (this.options.avatar !== "") {
                    var avatarElement = document.createElement("img");
                    avatarElement.src = this.options.avatar;

                    avatarElement.className = "toastify-avatar";

                    if (this.options.position == "left" || this.options.positionLeft === true) {
                        // Adding close icon on the left of content
                        divElement.appendChild(avatarElement);
                    } else {
                        // Adding close icon on the right of content
                        divElement.insertAdjacentElement("beforeend", avatarElement);
                    }
                }
            }

            // Adding a close icon to the toast
            if (this.options.close === true) {
                // Create a span for close element
                var closeElement = document.createElement("span");
                closeElement.innerHTML = "X";

                closeElement.className = "toast-close";

                // Triggering the removal of toast from DOM on close click
                closeElement.addEventListener(
                    "click",
                    function (event) {
                        event.stopPropagation();
                        this.removeElement(this.toastElement);
                        window.clearTimeout(this.toastElement.timeOutValue);
                    }.bind(this)
                );

                //Calculating screen width
                var width = window.innerWidth > 0 ? window.innerWidth : screen.width;

                // Adding the close icon to the toast element
                // Display on the right if screen width is less than or equal to 360px
                if ((this.options.position == "left" || this.options.positionLeft === true) && width > 360) {
                    // Adding close icon on the left of content
                    divElement.insertAdjacentElement("afterbegin", closeElement);
                } else {
                    // Adding close icon on the right of content
                    divElement.appendChild(closeElement);
                }
            }

            // Clear timeout while toast is focused
            if (this.options.stopOnFocus && this.options.duration > 0) {
                const self = this;
                // stop countdown
                divElement.addEventListener(
                    "mouseover",
                    function (event) {
                        window.clearTimeout(divElement.timeOutValue);
                    }
                )
                // add back the timeout
                divElement.addEventListener(
                    "mouseleave",
                    function () {
                        divElement.timeOutValue = window.setTimeout(
                            function () {
                                // Remove the toast from DOM
                                self.removeElement(divElement);
                            },
                            self.options.duration
                        )
                    }
                )
            }

            // Adding an on-click destination path
            if (typeof this.options.destination !== "undefined") {
                divElement.addEventListener(
                    "click",
                    function (event) {
                        event.stopPropagation();
                        if (this.options.newWindow === true) {
                            window.open(this.options.destination, "_blank");
                        } else {
                            window.location = this.options.destination;
                        }
                    }.bind(this)
                );
            }

            if (typeof this.options.onClick === "function" && typeof this.options.destination === "undefined") {
                divElement.addEventListener(
                    "click",
                    function (event) {
                        event.stopPropagation();
                        this.options.onClick();
                    }.bind(this)
                );
            }

            // Returning the generated element
            return divElement;
        },

        // Displaying the toast
        showToast: function () {
            // Creating the DOM object for the toast
            this.toastElement = this.buildToast();

            // Getting the root element to with the toast needs to be added
            var rootElement;
            if (typeof this.options.selector === "undefined") {
                rootElement = document.body;
            } else {
                rootElement = document.getElementById(this.options.selector);
            }

            // Validating if root element is present in DOM
            if (!rootElement) {
                throw "Root element is not defined";
            }

            // Adding the DOM element
            rootElement.insertBefore(this.toastElement, rootElement.firstChild);

            // Repositioning the toasts in case multiple toasts are present
            Toastify.reposition();

            if (this.options.duration > 0) {
                this.toastElement.timeOutValue = window.setTimeout(
                    function () {
                        // Remove the toast from DOM
                        this.removeElement(this.toastElement);
                    }.bind(this),
                    this.options.duration
                ); // Binding `this` for function invocation
            }

            // Supporting function chaining
            return this;
        },

        hideToast: function () {
            if (this.toastElement.timeOutValue) {
                clearTimeout(this.toastElement.timeOutValue);
            }
            this.removeElement(this.toastElement);
        },

        // Removing the element from the DOM
        removeElement: function (toastElement) {
            // Hiding the element
            // toastElement.classList.remove("on");
            toastElement.className = toastElement.className.replace(" on", "");

            // Removing the element from DOM after transition end
            window.setTimeout(
                function () {
                    // remove options node if any
                    if (this.options.node && this.options.node.parentNode) {
                        this.options.node.parentNode.removeChild(this.options.node);
                    }

                    // Remove the elemenf from the DOM, only when the parent node was not removed before.
                    if (toastElement.parentNode) {
                        toastElement.parentNode.removeChild(toastElement);
                    }

                    // Calling the callback function
                    this.options.callback.call(toastElement);

                    // Repositioning the toasts again
                    Toastify.reposition();
                }.bind(this),
                400
            ); // Binding `this` for function invocation
        },
    };

    // Positioning the toasts on the DOM
    Toastify.reposition = function () {
        // Top margins with gravity
        var topLeftOffsetSize = {
            top: 15,
            bottom: 15,
        };
        var topRightOffsetSize = {
            top: 15,
            bottom: 15,
        };
        var offsetSize = {
            top: 15,
            bottom: 15,
        };

        // Get all toast messages on the DOM
        var allToasts = document.getElementsByClassName("toastify");

        var classUsed;

        // Modifying the position of each toast element
        for (var i = 0; i < allToasts.length; i++) {
            // Getting the applied gravity
            if (containsClass(allToasts[i], "toastify-top") === true) {
                classUsed = "toastify-top";
            } else {
                classUsed = "toastify-bottom";
            }

            var height = allToasts[i].offsetHeight;
            classUsed = classUsed.substr(9, classUsed.length - 1)
            // Spacing between toasts
            var offset = 15;

            var width = window.innerWidth > 0 ? window.innerWidth : screen.width;

            // Show toast in center if screen with less than or qual to 360px
            if (width <= 360) {
                // Setting the position
                allToasts[i].style[classUsed] = offsetSize[classUsed] + "px";

                offsetSize[classUsed] += height + offset;
            } else {
                if (containsClass(allToasts[i], "toastify-left") === true) {
                    // Setting the position
                    allToasts[i].style[classUsed] = topLeftOffsetSize[classUsed] + "px";

                    topLeftOffsetSize[classUsed] += height + offset;
                } else {
                    // Setting the position
                    allToasts[i].style[classUsed] = topRightOffsetSize[classUsed] + "px";

                    topRightOffsetSize[classUsed] += height + offset;
                }
            }
        }

        // Supporting function chaining
        return this;
    };

    function containsClass(elem, yourClass) {
        if (!elem || typeof yourClass !== "string") {
            return false;
        } else if (
            elem.className &&
            elem.className
                .trim()
                .split(/\s+/gi)
                .indexOf(yourClass) > -1
        ) {
            return true;
        } else {
            return false;
        }
    }

    // Setting up the prototype for the init object
    Toastify.lib.init.prototype = Toastify.lib;

    // Returning the Toastify function to be assigned to the window object/module
    return Toastify;
});

/* flatpickr v4.6.9,, @license MIT */
!function (e, t) { "object" == typeof exports && "undefined" != typeof module ? module.exports = t() : "function" == typeof define && define.amd ? define(t) : (e = "undefined" != typeof globalThis ? globalThis : e || self).flatpickr = t() }(this, (function () { "use strict"; var e = function () { return (e = Object.assign || function (e) { for (var t, n = 1, a = arguments.length; n < a; n++)for (var i in t = arguments[n]) Object.prototype.hasOwnProperty.call(t, i) && (e[i] = t[i]); return e }).apply(this, arguments) }; function t() { for (var e = 0, t = 0, n = arguments.length; t < n; t++)e += arguments[t].length; var a = Array(e), i = 0; for (t = 0; t < n; t++)for (var o = arguments[t], r = 0, l = o.length; r < l; r++, i++)a[i] = o[r]; return a } var n = ["onChange", "onClose", "onDayCreate", "onDestroy", "onKeyDown", "onMonthChange", "onOpen", "onParseConfig", "onReady", "onValueUpdate", "onYearChange", "onPreCalendarPosition"], a = { _disable: [], allowInput: !1, allowInvalidPreload: !1, altFormat: "F j, Y", altInput: !1, altInputClass: "form-control input", animate: "object" == typeof window && -1 === window.navigator.userAgent.indexOf("MSIE"), ariaDateFormat: "F j, Y", autoFillDefaultTime: !0, clickOpens: !0, closeOnSelect: !0, conjunction: ", ", dateFormat: "Y-m-d", defaultHour: 12, defaultMinute: 0, defaultSeconds: 0, disable: [], disableMobile: !1, enableSeconds: !1, enableTime: !1, errorHandler: function (e) { return "undefined" != typeof console && console.warn(e) }, getWeek: function (e) { var t = new Date(e.getTime()); t.setHours(0, 0, 0, 0), t.setDate(t.getDate() + 3 - (t.getDay() + 6) % 7); var n = new Date(t.getFullYear(), 0, 4); return 1 + Math.round(((t.getTime() - n.getTime()) / 864e5 - 3 + (n.getDay() + 6) % 7) / 7) }, hourIncrement: 1, ignoredFocusElements: [], inline: !1, locale: "default", minuteIncrement: 5, mode: "single", monthSelectorType: "dropdown", nextArrow: "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 17 17'><g></g><path d='M13.207 8.472l-7.854 7.854-0.707-0.707 7.146-7.146-7.146-7.148 0.707-0.707 7.854 7.854z' /></svg>", noCalendar: !1, now: new Date, onChange: [], onClose: [], onDayCreate: [], onDestroy: [], onKeyDown: [], onMonthChange: [], onOpen: [], onParseConfig: [], onReady: [], onValueUpdate: [], onYearChange: [], onPreCalendarPosition: [], plugins: [], position: "auto", positionElement: void 0, prevArrow: "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' viewBox='0 0 17 17'><g></g><path d='M5.207 8.471l7.146 7.147-0.707 0.707-7.853-7.854 7.854-7.853 0.707 0.707-7.147 7.146z' /></svg>", shorthandCurrentMonth: !1, showMonths: 1, static: !1, time_24hr: !1, weekNumbers: !1, wrap: !1 }, i = { weekdays: { shorthand: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], longhand: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"] }, months: { shorthand: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], longhand: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"] }, daysInMonth: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31], firstDayOfWeek: 0, ordinal: function (e) { var t = e % 100; if (t > 3 && t < 21) return "th"; switch (t % 10) { case 1: return "st"; case 2: return "nd"; case 3: return "rd"; default: return "th" } }, rangeSeparator: " to ", weekAbbreviation: "Wk", scrollTitle: "Scroll to increment", toggleTitle: "Click to toggle", amPM: ["AM", "PM"], yearAriaLabel: "Year", monthAriaLabel: "Month", hourAriaLabel: "Hour", minuteAriaLabel: "Minute", time_24hr: !1 }, o = function (e, t) { return void 0 === t && (t = 2), ("000" + e).slice(-1 * t) }, r = function (e) { return !0 === e ? 1 : 0 }; function l(e, t) { var n; return function () { var a = this; clearTimeout(n), n = setTimeout((function () { return e.apply(a, arguments) }), t) } } var c = function (e) { return e instanceof Array ? e : [e] }; function d(e, t, n) { if (!0 === n) return e.classList.add(t); e.classList.remove(t) } function s(e, t, n) { var a = window.document.createElement(e); return t = t || "", n = n || "", a.className = t, void 0 !== n && (a.textContent = n), a } function u(e) { for (; e.firstChild;)e.removeChild(e.firstChild) } function f(e, t) { return t(e) ? e : e.parentNode ? f(e.parentNode, t) : void 0 } function m(e, t) { var n = s("div", "numInputWrapper"), a = s("input", "numInput " + e), i = s("span", "arrowUp"), o = s("span", "arrowDown"); if (-1 === navigator.userAgent.indexOf("MSIE 9.0") ? a.type = "number" : (a.type = "text", a.pattern = "\\d*"), void 0 !== t) for (var r in t) a.setAttribute(r, t[r]); return n.appendChild(a), n.appendChild(i), n.appendChild(o), n } function g(e) { try { return "function" == typeof e.composedPath ? e.composedPath()[0] : e.target } catch (t) { return e.target } } var p = function () { }, h = function (e, t, n) { return n.months[t ? "shorthand" : "longhand"][e] }, v = { D: p, F: function (e, t, n) { e.setMonth(n.months.longhand.indexOf(t)) }, G: function (e, t) { e.setHours(parseFloat(t)) }, H: function (e, t) { e.setHours(parseFloat(t)) }, J: function (e, t) { e.setDate(parseFloat(t)) }, K: function (e, t, n) { e.setHours(e.getHours() % 12 + 12 * r(new RegExp(n.amPM[1], "i").test(t))) }, M: function (e, t, n) { e.setMonth(n.months.shorthand.indexOf(t)) }, S: function (e, t) { e.setSeconds(parseFloat(t)) }, U: function (e, t) { return new Date(1e3 * parseFloat(t)) }, W: function (e, t, n) { var a = parseInt(t), i = new Date(e.getFullYear(), 0, 2 + 7 * (a - 1), 0, 0, 0, 0); return i.setDate(i.getDate() - i.getDay() + n.firstDayOfWeek), i }, Y: function (e, t) { e.setFullYear(parseFloat(t)) }, Z: function (e, t) { return new Date(t) }, d: function (e, t) { e.setDate(parseFloat(t)) }, h: function (e, t) { e.setHours(parseFloat(t)) }, i: function (e, t) { e.setMinutes(parseFloat(t)) }, j: function (e, t) { e.setDate(parseFloat(t)) }, l: p, m: function (e, t) { e.setMonth(parseFloat(t) - 1) }, n: function (e, t) { e.setMonth(parseFloat(t) - 1) }, s: function (e, t) { e.setSeconds(parseFloat(t)) }, u: function (e, t) { return new Date(parseFloat(t)) }, w: p, y: function (e, t) { e.setFullYear(2e3 + parseFloat(t)) } }, D = { D: "(\\w+)", F: "(\\w+)", G: "(\\d\\d|\\d)", H: "(\\d\\d|\\d)", J: "(\\d\\d|\\d)\\w+", K: "", M: "(\\w+)", S: "(\\d\\d|\\d)", U: "(.+)", W: "(\\d\\d|\\d)", Y: "(\\d{4})", Z: "(.+)", d: "(\\d\\d|\\d)", h: "(\\d\\d|\\d)", i: "(\\d\\d|\\d)", j: "(\\d\\d|\\d)", l: "(\\w+)", m: "(\\d\\d|\\d)", n: "(\\d\\d|\\d)", s: "(\\d\\d|\\d)", u: "(.+)", w: "(\\d\\d|\\d)", y: "(\\d{2})" }, w = { Z: function (e) { return e.toISOString() }, D: function (e, t, n) { return t.weekdays.shorthand[w.w(e, t, n)] }, F: function (e, t, n) { return h(w.n(e, t, n) - 1, !1, t) }, G: function (e, t, n) { return o(w.h(e, t, n)) }, H: function (e) { return o(e.getHours()) }, J: function (e, t) { return void 0 !== t.ordinal ? e.getDate() + t.ordinal(e.getDate()) : e.getDate() }, K: function (e, t) { return t.amPM[r(e.getHours() > 11)] }, M: function (e, t) { return h(e.getMonth(), !0, t) }, S: function (e) { return o(e.getSeconds()) }, U: function (e) { return e.getTime() / 1e3 }, W: function (e, t, n) { return n.getWeek(e) }, Y: function (e) { return o(e.getFullYear(), 4) }, d: function (e) { return o(e.getDate()) }, h: function (e) { return e.getHours() % 12 ? e.getHours() % 12 : 12 }, i: function (e) { return o(e.getMinutes()) }, j: function (e) { return e.getDate() }, l: function (e, t) { return t.weekdays.longhand[e.getDay()] }, m: function (e) { return o(e.getMonth() + 1) }, n: function (e) { return e.getMonth() + 1 }, s: function (e) { return e.getSeconds() }, u: function (e) { return e.getTime() }, w: function (e) { return e.getDay() }, y: function (e) { return String(e.getFullYear()).substring(2) } }, b = function (e) { var t = e.config, n = void 0 === t ? a : t, o = e.l10n, r = void 0 === o ? i : o, l = e.isMobile, c = void 0 !== l && l; return function (e, t, a) { var i = a || r; return void 0 === n.formatDate || c ? t.split("").map((function (t, a, o) { return w[t] && "\\" !== o[a - 1] ? w[t](e, i, n) : "\\" !== t ? t : "" })).join("") : n.formatDate(e, t, i) } }, C = function (e) { var t = e.config, n = void 0 === t ? a : t, o = e.l10n, r = void 0 === o ? i : o; return function (e, t, i, o) { if (0 === e || e) { var l, c = o || r, d = e; if (e instanceof Date) l = new Date(e.getTime()); else if ("string" != typeof e && void 0 !== e.toFixed) l = new Date(e); else if ("string" == typeof e) { var s = t || (n || a).dateFormat, u = String(e).trim(); if ("today" === u) l = new Date, i = !0; else if (/Z$/.test(u) || /GMT$/.test(u)) l = new Date(e); else if (n && n.parseDate) l = n.parseDate(e, s); else { l = n && n.noCalendar ? new Date((new Date).setHours(0, 0, 0, 0)) : new Date((new Date).getFullYear(), 0, 1, 0, 0, 0, 0); for (var f = void 0, m = [], g = 0, p = 0, h = ""; g < s.length; g++) { var w = s[g], b = "\\" === w, C = "\\" === s[g - 1] || b; if (D[w] && !C) { h += D[w]; var M = new RegExp(h).exec(e); M && (f = !0) && m["Y" !== w ? "push" : "unshift"]({ fn: v[w], val: M[++p] }) } else b || (h += "."); m.forEach((function (e) { var t = e.fn, n = e.val; return l = t(l, n, c) || l })) } l = f ? l : void 0 } } if (l instanceof Date && !isNaN(l.getTime())) return !0 === i && l.setHours(0, 0, 0, 0), l; n.errorHandler(new Error("Invalid date provided: " + d)) } } }; function M(e, t, n) { return void 0 === n && (n = !0), !1 !== n ? new Date(e.getTime()).setHours(0, 0, 0, 0) - new Date(t.getTime()).setHours(0, 0, 0, 0) : e.getTime() - t.getTime() } var y = 864e5; function x(e) { var t = e.defaultHour, n = e.defaultMinute, a = e.defaultSeconds; if (void 0 !== e.minDate) { var i = e.minDate.getHours(), o = e.minDate.getMinutes(), r = e.minDate.getSeconds(); t < i && (t = i), t === i && n < o && (n = o), t === i && n === o && a < r && (a = e.minDate.getSeconds()) } if (void 0 !== e.maxDate) { var l = e.maxDate.getHours(), c = e.maxDate.getMinutes(); (t = Math.min(t, l)) === l && (n = Math.min(c, n)), t === l && n === c && (a = e.maxDate.getSeconds()) } return { hours: t, minutes: n, seconds: a } } "function" != typeof Object.assign && (Object.assign = function (e) { for (var t = [], n = 1; n < arguments.length; n++)t[n - 1] = arguments[n]; if (!e) throw TypeError("Cannot convert undefined or null to object"); for (var a = function (t) { t && Object.keys(t).forEach((function (n) { return e[n] = t[n] })) }, i = 0, o = t; i < o.length; i++) { var r = o[i]; a(r) } return e }); function E(p, v) { var w = { config: e(e({}, a), T.defaultConfig), l10n: i }; function E(e) { return e.bind(w) } function k() { var e = w.config; !1 === e.weekNumbers && 1 === e.showMonths || !0 !== e.noCalendar && window.requestAnimationFrame((function () { if (void 0 !== w.calendarContainer && (w.calendarContainer.style.visibility = "hidden", w.calendarContainer.style.display = "block"), void 0 !== w.daysContainer) { var t = (w.days.offsetWidth + 1) * e.showMonths; w.daysContainer.style.width = t + "px", w.calendarContainer.style.width = t + (void 0 !== w.weekWrapper ? w.weekWrapper.offsetWidth : 0) + "px", w.calendarContainer.style.removeProperty("visibility"), w.calendarContainer.style.removeProperty("display") } })) } function I(e) { if (0 === w.selectedDates.length) { var t = void 0 === w.config.minDate || M(new Date, w.config.minDate) >= 0 ? new Date : new Date(w.config.minDate.getTime()), n = x(w.config); t.setHours(n.hours, n.minutes, n.seconds, t.getMilliseconds()), w.selectedDates = [t], w.latestSelectedDateObj = t } void 0 !== e && "blur" !== e.type && function (e) { e.preventDefault(); var t = "keydown" === e.type, n = g(e), a = n; void 0 !== w.amPM && n === w.amPM && (w.amPM.textContent = w.l10n.amPM[r(w.amPM.textContent === w.l10n.amPM[0])]); var i = parseFloat(a.getAttribute("min")), l = parseFloat(a.getAttribute("max")), c = parseFloat(a.getAttribute("step")), d = parseInt(a.value, 10), s = e.delta || (t ? 38 === e.which ? 1 : -1 : 0), u = d + c * s; if (void 0 !== a.value && 2 === a.value.length) { var f = a === w.hourElement, m = a === w.minuteElement; u < i ? (u = l + u + r(!f) + (r(f) && r(!w.amPM)), m && j(void 0, -1, w.hourElement)) : u > l && (u = a === w.hourElement ? u - l - r(!w.amPM) : i, m && j(void 0, 1, w.hourElement)), w.amPM && f && (1 === c ? u + d === 23 : Math.abs(u - d) > c) && (w.amPM.textContent = w.l10n.amPM[r(w.amPM.textContent === w.l10n.amPM[0])]), a.value = o(u) } }(e); var a = w._input.value; S(), be(), w._input.value !== a && w._debouncedChange() } function S() { if (void 0 !== w.hourElement && void 0 !== w.minuteElement) { var e, t, n = (parseInt(w.hourElement.value.slice(-2), 10) || 0) % 24, a = (parseInt(w.minuteElement.value, 10) || 0) % 60, i = void 0 !== w.secondElement ? (parseInt(w.secondElement.value, 10) || 0) % 60 : 0; void 0 !== w.amPM && (e = n, t = w.amPM.textContent, n = e % 12 + 12 * r(t === w.l10n.amPM[1])); var o = void 0 !== w.config.minTime || w.config.minDate && w.minDateHasTime && w.latestSelectedDateObj && 0 === M(w.latestSelectedDateObj, w.config.minDate, !0); if (void 0 !== w.config.maxTime || w.config.maxDate && w.maxDateHasTime && w.latestSelectedDateObj && 0 === M(w.latestSelectedDateObj, w.config.maxDate, !0)) { var l = void 0 !== w.config.maxTime ? w.config.maxTime : w.config.maxDate; (n = Math.min(n, l.getHours())) === l.getHours() && (a = Math.min(a, l.getMinutes())), a === l.getMinutes() && (i = Math.min(i, l.getSeconds())) } if (o) { var c = void 0 !== w.config.minTime ? w.config.minTime : w.config.minDate; (n = Math.max(n, c.getHours())) === c.getHours() && a < c.getMinutes() && (a = c.getMinutes()), a === c.getMinutes() && (i = Math.max(i, c.getSeconds())) } O(n, a, i) } } function _(e) { var t = e || w.latestSelectedDateObj; t && O(t.getHours(), t.getMinutes(), t.getSeconds()) } function O(e, t, n) { void 0 !== w.latestSelectedDateObj && w.latestSelectedDateObj.setHours(e % 24, t, n || 0, 0), w.hourElement && w.minuteElement && !w.isMobile && (w.hourElement.value = o(w.config.time_24hr ? e : (12 + e) % 12 + 12 * r(e % 12 == 0)), w.minuteElement.value = o(t), void 0 !== w.amPM && (w.amPM.textContent = w.l10n.amPM[r(e >= 12)]), void 0 !== w.secondElement && (w.secondElement.value = o(n))) } function F(e) { var t = g(e), n = parseInt(t.value) + (e.delta || 0); (n / 1e3 > 1 || "Enter" === e.key && !/[^\d]/.test(n.toString())) && Q(n) } function A(e, t, n, a) { return t instanceof Array ? t.forEach((function (t) { return A(e, t, n, a) })) : e instanceof Array ? e.forEach((function (e) { return A(e, t, n, a) })) : (e.addEventListener(t, n, a), void w._handlers.push({ remove: function () { return e.removeEventListener(t, n) } })) } function N() { pe("onChange") } function P(e, t) { var n = void 0 !== e ? w.parseDate(e) : w.latestSelectedDateObj || (w.config.minDate && w.config.minDate > w.now ? w.config.minDate : w.config.maxDate && w.config.maxDate < w.now ? w.config.maxDate : w.now), a = w.currentYear, i = w.currentMonth; try { void 0 !== n && (w.currentYear = n.getFullYear(), w.currentMonth = n.getMonth()) } catch (e) { e.message = "Invalid date supplied: " + n, w.config.errorHandler(e) } t && w.currentYear !== a && (pe("onYearChange"), K()), !t || w.currentYear === a && w.currentMonth === i || pe("onMonthChange"), w.redraw() } function Y(e) { var t = g(e); ~t.className.indexOf("arrow") && j(e, t.classList.contains("arrowUp") ? 1 : -1) } function j(e, t, n) { var a = e && g(e), i = n || a && a.parentNode && a.parentNode.firstChild, o = he("increment"); o.delta = t, i && i.dispatchEvent(o) } function H(e, t, n, a) { var i = X(t, !0), o = s("span", "flatpickr-day " + e, t.getDate().toString()); return o.dateObj = t, o.$i = a, o.setAttribute("aria-label", w.formatDate(t, w.config.ariaDateFormat)), -1 === e.indexOf("hidden") && 0 === M(t, w.now) && (w.todayDateElem = o, o.classList.add("today"), o.setAttribute("aria-current", "date")), i ? (o.tabIndex = -1, ve(t) && (o.classList.add("selected"), w.selectedDateElem = o, "range" === w.config.mode && (d(o, "startRange", w.selectedDates[0] && 0 === M(t, w.selectedDates[0], !0)), d(o, "endRange", w.selectedDates[1] && 0 === M(t, w.selectedDates[1], !0)), "nextMonthDay" === e && o.classList.add("inRange")))) : o.classList.add("flatpickr-disabled"), "range" === w.config.mode && function (e) { return !("range" !== w.config.mode || w.selectedDates.length < 2) && (M(e, w.selectedDates[0]) >= 0 && M(e, w.selectedDates[1]) <= 0) }(t) && !ve(t) && o.classList.add("inRange"), w.weekNumbers && 1 === w.config.showMonths && "prevMonthDay" !== e && n % 7 == 1 && w.weekNumbers.insertAdjacentHTML("beforeend", "<span class='flatpickr-day'>" + w.config.getWeek(t) + "</span>"), pe("onDayCreate", o), o } function L(e) { e.focus(), "range" === w.config.mode && ae(e) } function W(e) { for (var t = e > 0 ? 0 : w.config.showMonths - 1, n = e > 0 ? w.config.showMonths : -1, a = t; a != n; a += e)for (var i = w.daysContainer.children[a], o = e > 0 ? 0 : i.children.length - 1, r = e > 0 ? i.children.length : -1, l = o; l != r; l += e) { var c = i.children[l]; if (-1 === c.className.indexOf("hidden") && X(c.dateObj)) return c } } function R(e, t) { var n = ee(document.activeElement || document.body), a = void 0 !== e ? e : n ? document.activeElement : void 0 !== w.selectedDateElem && ee(w.selectedDateElem) ? w.selectedDateElem : void 0 !== w.todayDateElem && ee(w.todayDateElem) ? w.todayDateElem : W(t > 0 ? 1 : -1); void 0 === a ? w._input.focus() : n ? function (e, t) { for (var n = -1 === e.className.indexOf("Month") ? e.dateObj.getMonth() : w.currentMonth, a = t > 0 ? w.config.showMonths : -1, i = t > 0 ? 1 : -1, o = n - w.currentMonth; o != a; o += i)for (var r = w.daysContainer.children[o], l = n - w.currentMonth === o ? e.$i + t : t < 0 ? r.children.length - 1 : 0, c = r.children.length, d = l; d >= 0 && d < c && d != (t > 0 ? c : -1); d += i) { var s = r.children[d]; if (-1 === s.className.indexOf("hidden") && X(s.dateObj) && Math.abs(e.$i - d) >= Math.abs(t)) return L(s) } w.changeMonth(i), R(W(i), 0) }(a, t) : L(a) } function B(e, t) { for (var n = (new Date(e, t, 1).getDay() - w.l10n.firstDayOfWeek + 7) % 7, a = w.utils.getDaysInMonth((t - 1 + 12) % 12, e), i = w.utils.getDaysInMonth(t, e), o = window.document.createDocumentFragment(), r = w.config.showMonths > 1, l = r ? "prevMonthDay hidden" : "prevMonthDay", c = r ? "nextMonthDay hidden" : "nextMonthDay", d = a + 1 - n, u = 0; d <= a; d++, u++)o.appendChild(H(l, new Date(e, t - 1, d), d, u)); for (d = 1; d <= i; d++, u++)o.appendChild(H("", new Date(e, t, d), d, u)); for (var f = i + 1; f <= 42 - n && (1 === w.config.showMonths || u % 7 != 0); f++, u++)o.appendChild(H(c, new Date(e, t + 1, f % i), f, u)); var m = s("div", "dayContainer"); return m.appendChild(o), m } function J() { if (void 0 !== w.daysContainer) { u(w.daysContainer), w.weekNumbers && u(w.weekNumbers); for (var e = document.createDocumentFragment(), t = 0; t < w.config.showMonths; t++) { var n = new Date(w.currentYear, w.currentMonth, 1); n.setMonth(w.currentMonth + t), e.appendChild(B(n.getFullYear(), n.getMonth())) } w.daysContainer.appendChild(e), w.days = w.daysContainer.firstChild, "range" === w.config.mode && 1 === w.selectedDates.length && ae() } } function K() { if (!(w.config.showMonths > 1 || "dropdown" !== w.config.monthSelectorType)) { var e = function (e) { return !(void 0 !== w.config.minDate && w.currentYear === w.config.minDate.getFullYear() && e < w.config.minDate.getMonth()) && !(void 0 !== w.config.maxDate && w.currentYear === w.config.maxDate.getFullYear() && e > w.config.maxDate.getMonth()) }; w.monthsDropdownContainer.tabIndex = -1, w.monthsDropdownContainer.innerHTML = ""; for (var t = 0; t < 12; t++)if (e(t)) { var n = s("option", "flatpickr-monthDropdown-month"); n.value = new Date(w.currentYear, t).getMonth().toString(), n.textContent = h(t, w.config.shorthandCurrentMonth, w.l10n), n.tabIndex = -1, w.currentMonth === t && (n.selected = !0), w.monthsDropdownContainer.appendChild(n) } } } function U() { var e, t = s("div", "flatpickr-month"), n = window.document.createDocumentFragment(); w.config.showMonths > 1 || "static" === w.config.monthSelectorType ? e = s("span", "cur-month") : (w.monthsDropdownContainer = s("select", "flatpickr-monthDropdown-months"), w.monthsDropdownContainer.setAttribute("aria-label", w.l10n.monthAriaLabel), A(w.monthsDropdownContainer, "change", (function (e) { var t = g(e), n = parseInt(t.value, 10); w.changeMonth(n - w.currentMonth), pe("onMonthChange") })), K(), e = w.monthsDropdownContainer); var a = m("cur-year", { tabindex: "-1" }), i = a.getElementsByTagName("input")[0]; i.setAttribute("aria-label", w.l10n.yearAriaLabel), w.config.minDate && i.setAttribute("min", w.config.minDate.getFullYear().toString()), w.config.maxDate && (i.setAttribute("max", w.config.maxDate.getFullYear().toString()), i.disabled = !!w.config.minDate && w.config.minDate.getFullYear() === w.config.maxDate.getFullYear()); var o = s("div", "flatpickr-current-month"); return o.appendChild(e), o.appendChild(a), n.appendChild(o), t.appendChild(n), { container: t, yearElement: i, monthElement: e } } function q() { u(w.monthNav), w.monthNav.appendChild(w.prevMonthNav), w.config.showMonths && (w.yearElements = [], w.monthElements = []); for (var e = w.config.showMonths; e--;) { var t = U(); w.yearElements.push(t.yearElement), w.monthElements.push(t.monthElement), w.monthNav.appendChild(t.container) } w.monthNav.appendChild(w.nextMonthNav) } function $() { w.weekdayContainer ? u(w.weekdayContainer) : w.weekdayContainer = s("div", "flatpickr-weekdays"); for (var e = w.config.showMonths; e--;) { var t = s("div", "flatpickr-weekdaycontainer"); w.weekdayContainer.appendChild(t) } return z(), w.weekdayContainer } function z() { if (w.weekdayContainer) { var e = w.l10n.firstDayOfWeek, n = t(w.l10n.weekdays.shorthand); e > 0 && e < n.length && (n = t(n.splice(e, n.length), n.splice(0, e))); for (var a = w.config.showMonths; a--;)w.weekdayContainer.children[a].innerHTML = "\n      <span class='flatpickr-weekday'>\n        " + n.join("</span><span class='flatpickr-weekday'>") + "\n      </span>\n      " } } function G(e, t) { void 0 === t && (t = !0); var n = t ? e : e - w.currentMonth; n < 0 && !0 === w._hidePrevMonthArrow || n > 0 && !0 === w._hideNextMonthArrow || (w.currentMonth += n, (w.currentMonth < 0 || w.currentMonth > 11) && (w.currentYear += w.currentMonth > 11 ? 1 : -1, w.currentMonth = (w.currentMonth + 12) % 12, pe("onYearChange"), K()), J(), pe("onMonthChange"), De()) } function V(e) { return !(!w.config.appendTo || !w.config.appendTo.contains(e)) || w.calendarContainer.contains(e) } function Z(e) { if (w.isOpen && !w.config.inline) { var t = g(e), n = V(t), a = t === w.input || t === w.altInput || w.element.contains(t) || e.path && e.path.indexOf && (~e.path.indexOf(w.input) || ~e.path.indexOf(w.altInput)), i = "blur" === e.type ? a && e.relatedTarget && !V(e.relatedTarget) : !a && !n && !V(e.relatedTarget), o = !w.config.ignoredFocusElements.some((function (e) { return e.contains(t) })); i && o && (void 0 !== w.timeContainer && void 0 !== w.minuteElement && void 0 !== w.hourElement && "" !== w.input.value && void 0 !== w.input.value && I(), w.close(), w.config && "range" === w.config.mode && 1 === w.selectedDates.length && (w.clear(!1), w.redraw())) } } function Q(e) { if (!(!e || w.config.minDate && e < w.config.minDate.getFullYear() || w.config.maxDate && e > w.config.maxDate.getFullYear())) { var t = e, n = w.currentYear !== t; w.currentYear = t || w.currentYear, w.config.maxDate && w.currentYear === w.config.maxDate.getFullYear() ? w.currentMonth = Math.min(w.config.maxDate.getMonth(), w.currentMonth) : w.config.minDate && w.currentYear === w.config.minDate.getFullYear() && (w.currentMonth = Math.max(w.config.minDate.getMonth(), w.currentMonth)), n && (w.redraw(), pe("onYearChange"), K()) } } function X(e, t) { var n; void 0 === t && (t = !0); var a = w.parseDate(e, void 0, t); if (w.config.minDate && a && M(a, w.config.minDate, void 0 !== t ? t : !w.minDateHasTime) < 0 || w.config.maxDate && a && M(a, w.config.maxDate, void 0 !== t ? t : !w.maxDateHasTime) > 0) return !1; if (!w.config.enable && 0 === w.config.disable.length) return !0; if (void 0 === a) return !1; for (var i = !!w.config.enable, o = null !== (n = w.config.enable) && void 0 !== n ? n : w.config.disable, r = 0, l = void 0; r < o.length; r++) { if ("function" == typeof (l = o[r]) && l(a)) return i; if (l instanceof Date && void 0 !== a && l.getTime() === a.getTime()) return i; if ("string" == typeof l) { var c = w.parseDate(l, void 0, !0); return c && c.getTime() === a.getTime() ? i : !i } if ("object" == typeof l && void 0 !== a && l.from && l.to && a.getTime() >= l.from.getTime() && a.getTime() <= l.to.getTime()) return i } return !i } function ee(e) { return void 0 !== w.daysContainer && (-1 === e.className.indexOf("hidden") && -1 === e.className.indexOf("flatpickr-disabled") && w.daysContainer.contains(e)) } function te(e) { !(e.target === w._input) || !(w.selectedDates.length > 0 || w._input.value.length > 0) || e.relatedTarget && V(e.relatedTarget) || w.setDate(w._input.value, !0, e.target === w.altInput ? w.config.altFormat : w.config.dateFormat) } function ne(e) { var t = g(e), n = w.config.wrap ? p.contains(t) : t === w._input, a = w.config.allowInput, i = w.isOpen && (!a || !n), o = w.config.inline && n && !a; if (13 === e.keyCode && n) { if (a) return w.setDate(w._input.value, !0, t === w.altInput ? w.config.altFormat : w.config.dateFormat), t.blur(); w.open() } else if (V(t) || i || o) { var r = !!w.timeContainer && w.timeContainer.contains(t); switch (e.keyCode) { case 13: r ? (e.preventDefault(), I(), se()) : ue(e); break; case 27: e.preventDefault(), se(); break; case 8: case 46: n && !w.config.allowInput && (e.preventDefault(), w.clear()); break; case 37: case 39: if (r || n) w.hourElement && w.hourElement.focus(); else if (e.preventDefault(), void 0 !== w.daysContainer && (!1 === a || document.activeElement && ee(document.activeElement))) { var l = 39 === e.keyCode ? 1 : -1; e.ctrlKey ? (e.stopPropagation(), G(l), R(W(1), 0)) : R(void 0, l) } break; case 38: case 40: e.preventDefault(); var c = 40 === e.keyCode ? 1 : -1; w.daysContainer && void 0 !== t.$i || t === w.input || t === w.altInput ? e.ctrlKey ? (e.stopPropagation(), Q(w.currentYear - c), R(W(1), 0)) : r || R(void 0, 7 * c) : t === w.currentYearElement ? Q(w.currentYear - c) : w.config.enableTime && (!r && w.hourElement && w.hourElement.focus(), I(e), w._debouncedChange()); break; case 9: if (r) { var d = [w.hourElement, w.minuteElement, w.secondElement, w.amPM].concat(w.pluginElements).filter((function (e) { return e })), s = d.indexOf(t); if (-1 !== s) { var u = d[s + (e.shiftKey ? -1 : 1)]; e.preventDefault(), (u || w._input).focus() } } else !w.config.noCalendar && w.daysContainer && w.daysContainer.contains(t) && e.shiftKey && (e.preventDefault(), w._input.focus()) } } if (void 0 !== w.amPM && t === w.amPM) switch (e.key) { case w.l10n.amPM[0].charAt(0): case w.l10n.amPM[0].charAt(0).toLowerCase(): w.amPM.textContent = w.l10n.amPM[0], S(), be(); break; case w.l10n.amPM[1].charAt(0): case w.l10n.amPM[1].charAt(0).toLowerCase(): w.amPM.textContent = w.l10n.amPM[1], S(), be() }(n || V(t)) && pe("onKeyDown", e) } function ae(e) { if (1 === w.selectedDates.length && (!e || e.classList.contains("flatpickr-day") && !e.classList.contains("flatpickr-disabled"))) { for (var t = e ? e.dateObj.getTime() : w.days.firstElementChild.dateObj.getTime(), n = w.parseDate(w.selectedDates[0], void 0, !0).getTime(), a = Math.min(t, w.selectedDates[0].getTime()), i = Math.max(t, w.selectedDates[0].getTime()), o = !1, r = 0, l = 0, c = a; c < i; c += y)X(new Date(c), !0) || (o = o || c > a && c < i, c < n && (!r || c > r) ? r = c : c > n && (!l || c < l) && (l = c)); for (var d = 0; d < w.config.showMonths; d++)for (var s = w.daysContainer.children[d], u = function (a, i) { var c, d, u, f = s.children[a], m = f.dateObj.getTime(), g = r > 0 && m < r || l > 0 && m > l; return g ? (f.classList.add("notAllowed"), ["inRange", "startRange", "endRange"].forEach((function (e) { f.classList.remove(e) })), "continue") : o && !g ? "continue" : (["startRange", "inRange", "endRange", "notAllowed"].forEach((function (e) { f.classList.remove(e) })), void (void 0 !== e && (e.classList.add(t <= w.selectedDates[0].getTime() ? "startRange" : "endRange"), n < t && m === n ? f.classList.add("startRange") : n > t && m === n && f.classList.add("endRange"), m >= r && (0 === l || m <= l) && (d = n, u = t, (c = m) > Math.min(d, u) && c < Math.max(d, u)) && f.classList.add("inRange")))) }, f = 0, m = s.children.length; f < m; f++)u(f) } } function ie() { !w.isOpen || w.config.static || w.config.inline || ce() } function oe(e) { return function (t) { var n = w.config["_" + e + "Date"] = w.parseDate(t, w.config.dateFormat), a = w.config["_" + ("min" === e ? "max" : "min") + "Date"]; void 0 !== n && (w["min" === e ? "minDateHasTime" : "maxDateHasTime"] = n.getHours() > 0 || n.getMinutes() > 0 || n.getSeconds() > 0), w.selectedDates && (w.selectedDates = w.selectedDates.filter((function (e) { return X(e) })), w.selectedDates.length || "min" !== e || _(n), be()), w.daysContainer && (de(), void 0 !== n ? w.currentYearElement[e] = n.getFullYear().toString() : w.currentYearElement.removeAttribute(e), w.currentYearElement.disabled = !!a && void 0 !== n && a.getFullYear() === n.getFullYear()) } } function re() { return w.config.wrap ? p.querySelector("[data-input]") : p } function le() { "object" != typeof w.config.locale && void 0 === T.l10ns[w.config.locale] && w.config.errorHandler(new Error("flatpickr: invalid locale " + w.config.locale)), w.l10n = e(e({}, T.l10ns.default), "object" == typeof w.config.locale ? w.config.locale : "default" !== w.config.locale ? T.l10ns[w.config.locale] : void 0), D.K = "(" + w.l10n.amPM[0] + "|" + w.l10n.amPM[1] + "|" + w.l10n.amPM[0].toLowerCase() + "|" + w.l10n.amPM[1].toLowerCase() + ")", void 0 === e(e({}, v), JSON.parse(JSON.stringify(p.dataset || {}))).time_24hr && void 0 === T.defaultConfig.time_24hr && (w.config.time_24hr = w.l10n.time_24hr), w.formatDate = b(w), w.parseDate = C({ config: w.config, l10n: w.l10n }) } function ce(e) { if ("function" != typeof w.config.position) { if (void 0 !== w.calendarContainer) { pe("onPreCalendarPosition"); var t = e || w._positionElement, n = Array.prototype.reduce.call(w.calendarContainer.children, (function (e, t) { return e + t.offsetHeight }), 0), a = w.calendarContainer.offsetWidth, i = w.config.position.split(" "), o = i[0], r = i.length > 1 ? i[1] : null, l = t.getBoundingClientRect(), c = window.innerHeight - l.bottom, s = "above" === o || "below" !== o && c < n && l.top > n, u = window.pageYOffset + l.top + (s ? -n - 2 : t.offsetHeight + 2); if (d(w.calendarContainer, "arrowTop", !s), d(w.calendarContainer, "arrowBottom", s), !w.config.inline) { var f = window.pageXOffset + l.left, m = !1, g = !1; "center" === r ? (f -= (a - l.width) / 2, m = !0) : "right" === r && (f -= a - l.width, g = !0), d(w.calendarContainer, "arrowLeft", !m && !g), d(w.calendarContainer, "arrowCenter", m), d(w.calendarContainer, "arrowRight", g); var p = window.document.body.offsetWidth - (window.pageXOffset + l.right), h = f + a > window.document.body.offsetWidth, v = p + a > window.document.body.offsetWidth; if (d(w.calendarContainer, "rightMost", h), !w.config.static) if (w.calendarContainer.style.top = u + "px", h) if (v) { var D = function () { for (var e = null, t = 0; t < document.styleSheets.length; t++) { var n = document.styleSheets[t]; try { n.cssRules } catch (e) { continue } e = n; break } return null != e ? e : (a = document.createElement("style"), document.head.appendChild(a), a.sheet); var a }(); if (void 0 === D) return; var b = window.document.body.offsetWidth, C = Math.max(0, b / 2 - a / 2), M = D.cssRules.length, y = "{left:" + l.left + "px;right:auto;}"; d(w.calendarContainer, "rightMost", !1), d(w.calendarContainer, "centerMost", !0), D.insertRule(".flatpickr-calendar.centerMost:before,.flatpickr-calendar.centerMost:after" + y, M), w.calendarContainer.style.left = C + "px", w.calendarContainer.style.right = "auto" } else w.calendarContainer.style.left = "auto", w.calendarContainer.style.right = p + "px"; else w.calendarContainer.style.left = f + "px", w.calendarContainer.style.right = "auto" } } } else w.config.position(w, e) } function de() { w.config.noCalendar || w.isMobile || (K(), De(), J()) } function se() { w._input.focus(), -1 !== window.navigator.userAgent.indexOf("MSIE") || void 0 !== navigator.msMaxTouchPoints ? setTimeout(w.close, 0) : w.close() } function ue(e) { e.preventDefault(), e.stopPropagation(); var t = f(g(e), (function (e) { return e.classList && e.classList.contains("flatpickr-day") && !e.classList.contains("flatpickr-disabled") && !e.classList.contains("notAllowed") })); if (void 0 !== t) { var n = t, a = w.latestSelectedDateObj = new Date(n.dateObj.getTime()), i = (a.getMonth() < w.currentMonth || a.getMonth() > w.currentMonth + w.config.showMonths - 1) && "range" !== w.config.mode; if (w.selectedDateElem = n, "single" === w.config.mode) w.selectedDates = [a]; else if ("multiple" === w.config.mode) { var o = ve(a); o ? w.selectedDates.splice(parseInt(o), 1) : w.selectedDates.push(a) } else "range" === w.config.mode && (2 === w.selectedDates.length && w.clear(!1, !1), w.latestSelectedDateObj = a, w.selectedDates.push(a), 0 !== M(a, w.selectedDates[0], !0) && w.selectedDates.sort((function (e, t) { return e.getTime() - t.getTime() }))); if (S(), i) { var r = w.currentYear !== a.getFullYear(); w.currentYear = a.getFullYear(), w.currentMonth = a.getMonth(), r && (pe("onYearChange"), K()), pe("onMonthChange") } if (De(), J(), be(), i || "range" === w.config.mode || 1 !== w.config.showMonths ? void 0 !== w.selectedDateElem && void 0 === w.hourElement && w.selectedDateElem && w.selectedDateElem.focus() : L(n), void 0 !== w.hourElement && void 0 !== w.hourElement && w.hourElement.focus(), w.config.closeOnSelect) { var l = "single" === w.config.mode && !w.config.enableTime, c = "range" === w.config.mode && 2 === w.selectedDates.length && !w.config.enableTime; (l || c) && se() } N() } } w.parseDate = C({ config: w.config, l10n: w.l10n }), w._handlers = [], w.pluginElements = [], w.loadedPlugins = [], w._bind = A, w._setHoursFromDate = _, w._positionCalendar = ce, w.changeMonth = G, w.changeYear = Q, w.clear = function (e, t) { void 0 === e && (e = !0); void 0 === t && (t = !0); w.input.value = "", void 0 !== w.altInput && (w.altInput.value = ""); void 0 !== w.mobileInput && (w.mobileInput.value = ""); w.selectedDates = [], w.latestSelectedDateObj = void 0, !0 === t && (w.currentYear = w._initialDate.getFullYear(), w.currentMonth = w._initialDate.getMonth()); if (!0 === w.config.enableTime) { var n = x(w.config), a = n.hours, i = n.minutes, o = n.seconds; O(a, i, o) } w.redraw(), e && pe("onChange") }, w.close = function () { w.isOpen = !1, w.isMobile || (void 0 !== w.calendarContainer && w.calendarContainer.classList.remove("open"), void 0 !== w._input && w._input.classList.remove("active")); pe("onClose") }, w._createElement = s, w.destroy = function () { void 0 !== w.config && pe("onDestroy"); for (var e = w._handlers.length; e--;)w._handlers[e].remove(); if (w._handlers = [], w.mobileInput) w.mobileInput.parentNode && w.mobileInput.parentNode.removeChild(w.mobileInput), w.mobileInput = void 0; else if (w.calendarContainer && w.calendarContainer.parentNode) if (w.config.static && w.calendarContainer.parentNode) { var t = w.calendarContainer.parentNode; if (t.lastChild && t.removeChild(t.lastChild), t.parentNode) { for (; t.firstChild;)t.parentNode.insertBefore(t.firstChild, t); t.parentNode.removeChild(t) } } else w.calendarContainer.parentNode.removeChild(w.calendarContainer); w.altInput && (w.input.type = "text", w.altInput.parentNode && w.altInput.parentNode.removeChild(w.altInput), delete w.altInput); w.input && (w.input.type = w.input._type, w.input.classList.remove("flatpickr-input"), w.input.removeAttribute("readonly"));["_showTimeInput", "latestSelectedDateObj", "_hideNextMonthArrow", "_hidePrevMonthArrow", "__hideNextMonthArrow", "__hidePrevMonthArrow", "isMobile", "isOpen", "selectedDateElem", "minDateHasTime", "maxDateHasTime", "days", "daysContainer", "_input", "_positionElement", "innerContainer", "rContainer", "monthNav", "todayDateElem", "calendarContainer", "weekdayContainer", "prevMonthNav", "nextMonthNav", "monthsDropdownContainer", "currentMonthElement", "currentYearElement", "navigationCurrentMonth", "selectedDateElem", "config"].forEach((function (e) { try { delete w[e] } catch (e) { } })) }, w.isEnabled = X, w.jumpToDate = P, w.open = function (e, t) { void 0 === t && (t = w._positionElement); if (!0 === w.isMobile) { if (e) { e.preventDefault(); var n = g(e); n && n.blur() } return void 0 !== w.mobileInput && (w.mobileInput.focus(), w.mobileInput.click()), void pe("onOpen") } if (w._input.disabled || w.config.inline) return; var a = w.isOpen; w.isOpen = !0, a || (w.calendarContainer.classList.add("open"), w._input.classList.add("active"), pe("onOpen"), ce(t)); !0 === w.config.enableTime && !0 === w.config.noCalendar && (!1 !== w.config.allowInput || void 0 !== e && w.timeContainer.contains(e.relatedTarget) || setTimeout((function () { return w.hourElement.select() }), 50)) }, w.redraw = de, w.set = function (e, t) { if (null !== e && "object" == typeof e) for (var a in Object.assign(w.config, e), e) void 0 !== fe[a] && fe[a].forEach((function (e) { return e() })); else w.config[e] = t, void 0 !== fe[e] ? fe[e].forEach((function (e) { return e() })) : n.indexOf(e) > -1 && (w.config[e] = c(t)); w.redraw(), be(!0) }, w.setDate = function (e, t, n) { void 0 === t && (t = !1); void 0 === n && (n = w.config.dateFormat); if (0 !== e && !e || e instanceof Array && 0 === e.length) return w.clear(t); me(e, n), w.latestSelectedDateObj = w.selectedDates[w.selectedDates.length - 1], w.redraw(), P(void 0, t), _(), 0 === w.selectedDates.length && w.clear(!1); be(t), t && pe("onChange") }, w.toggle = function (e) { if (!0 === w.isOpen) return w.close(); w.open(e) }; var fe = { locale: [le, z], showMonths: [q, k, $], minDate: [P], maxDate: [P], clickOpens: [function () { !0 === w.config.clickOpens ? (A(w._input, "focus", w.open), A(w._input, "click", w.open)) : (w._input.removeEventListener("focus", w.open), w._input.removeEventListener("click", w.open)) }] }; function me(e, t) { var n = []; if (e instanceof Array) n = e.map((function (e) { return w.parseDate(e, t) })); else if (e instanceof Date || "number" == typeof e) n = [w.parseDate(e, t)]; else if ("string" == typeof e) switch (w.config.mode) { case "single": case "time": n = [w.parseDate(e, t)]; break; case "multiple": n = e.split(w.config.conjunction).map((function (e) { return w.parseDate(e, t) })); break; case "range": n = e.split(w.l10n.rangeSeparator).map((function (e) { return w.parseDate(e, t) })) } else w.config.errorHandler(new Error("Invalid date supplied: " + JSON.stringify(e))); w.selectedDates = w.config.allowInvalidPreload ? n : n.filter((function (e) { return e instanceof Date && X(e, !1) })), "range" === w.config.mode && w.selectedDates.sort((function (e, t) { return e.getTime() - t.getTime() })) } function ge(e) { return e.slice().map((function (e) { return "string" == typeof e || "number" == typeof e || e instanceof Date ? w.parseDate(e, void 0, !0) : e && "object" == typeof e && e.from && e.to ? { from: w.parseDate(e.from, void 0), to: w.parseDate(e.to, void 0) } : e })).filter((function (e) { return e })) } function pe(e, t) { if (void 0 !== w.config) { var n = w.config[e]; if (void 0 !== n && n.length > 0) for (var a = 0; n[a] && a < n.length; a++)n[a](w.selectedDates, w.input.value, w, t); "onChange" === e && (w.input.dispatchEvent(he("change")), w.input.dispatchEvent(he("input"))) } } function he(e) { var t = document.createEvent("Event"); return t.initEvent(e, !0, !0), t } function ve(e) { for (var t = 0; t < w.selectedDates.length; t++)if (0 === M(w.selectedDates[t], e)) return "" + t; return !1 } function De() { w.config.noCalendar || w.isMobile || !w.monthNav || (w.yearElements.forEach((function (e, t) { var n = new Date(w.currentYear, w.currentMonth, 1); n.setMonth(w.currentMonth + t), w.config.showMonths > 1 || "static" === w.config.monthSelectorType ? w.monthElements[t].textContent = h(n.getMonth(), w.config.shorthandCurrentMonth, w.l10n) + " " : w.monthsDropdownContainer.value = n.getMonth().toString(), e.value = n.getFullYear().toString() })), w._hidePrevMonthArrow = void 0 !== w.config.minDate && (w.currentYear === w.config.minDate.getFullYear() ? w.currentMonth <= w.config.minDate.getMonth() : w.currentYear < w.config.minDate.getFullYear()), w._hideNextMonthArrow = void 0 !== w.config.maxDate && (w.currentYear === w.config.maxDate.getFullYear() ? w.currentMonth + 1 > w.config.maxDate.getMonth() : w.currentYear > w.config.maxDate.getFullYear())) } function we(e) { return w.selectedDates.map((function (t) { return w.formatDate(t, e) })).filter((function (e, t, n) { return "range" !== w.config.mode || w.config.enableTime || n.indexOf(e) === t })).join("range" !== w.config.mode ? w.config.conjunction : w.l10n.rangeSeparator) } function be(e) { void 0 === e && (e = !0), void 0 !== w.mobileInput && w.mobileFormatStr && (w.mobileInput.value = void 0 !== w.latestSelectedDateObj ? w.formatDate(w.latestSelectedDateObj, w.mobileFormatStr) : ""), w.input.value = we(w.config.dateFormat), void 0 !== w.altInput && (w.altInput.value = we(w.config.altFormat)), !1 !== e && pe("onValueUpdate") } function Ce(e) { var t = g(e), n = w.prevMonthNav.contains(t), a = w.nextMonthNav.contains(t); n || a ? G(n ? -1 : 1) : w.yearElements.indexOf(t) >= 0 ? t.select() : t.classList.contains("arrowUp") ? w.changeYear(w.currentYear + 1) : t.classList.contains("arrowDown") && w.changeYear(w.currentYear - 1) } return function () { w.element = w.input = p, w.isOpen = !1, function () { var t = ["wrap", "weekNumbers", "allowInput", "allowInvalidPreload", "clickOpens", "time_24hr", "enableTime", "noCalendar", "altInput", "shorthandCurrentMonth", "inline", "static", "enableSeconds", "disableMobile"], i = e(e({}, JSON.parse(JSON.stringify(p.dataset || {}))), v), o = {}; w.config.parseDate = i.parseDate, w.config.formatDate = i.formatDate, Object.defineProperty(w.config, "enable", { get: function () { return w.config._enable }, set: function (e) { w.config._enable = ge(e) } }), Object.defineProperty(w.config, "disable", { get: function () { return w.config._disable }, set: function (e) { w.config._disable = ge(e) } }); var r = "time" === i.mode; if (!i.dateFormat && (i.enableTime || r)) { var l = T.defaultConfig.dateFormat || a.dateFormat; o.dateFormat = i.noCalendar || r ? "H:i" + (i.enableSeconds ? ":S" : "") : l + " H:i" + (i.enableSeconds ? ":S" : "") } if (i.altInput && (i.enableTime || r) && !i.altFormat) { var d = T.defaultConfig.altFormat || a.altFormat; o.altFormat = i.noCalendar || r ? "h:i" + (i.enableSeconds ? ":S K" : " K") : d + " h:i" + (i.enableSeconds ? ":S" : "") + " K" } Object.defineProperty(w.config, "minDate", { get: function () { return w.config._minDate }, set: oe("min") }), Object.defineProperty(w.config, "maxDate", { get: function () { return w.config._maxDate }, set: oe("max") }); var s = function (e) { return function (t) { w.config["min" === e ? "_minTime" : "_maxTime"] = w.parseDate(t, "H:i:S") } }; Object.defineProperty(w.config, "minTime", { get: function () { return w.config._minTime }, set: s("min") }), Object.defineProperty(w.config, "maxTime", { get: function () { return w.config._maxTime }, set: s("max") }), "time" === i.mode && (w.config.noCalendar = !0, w.config.enableTime = !0); Object.assign(w.config, o, i); for (var u = 0; u < t.length; u++)w.config[t[u]] = !0 === w.config[t[u]] || "true" === w.config[t[u]]; n.filter((function (e) { return void 0 !== w.config[e] })).forEach((function (e) { w.config[e] = c(w.config[e] || []).map(E) })), w.isMobile = !w.config.disableMobile && !w.config.inline && "single" === w.config.mode && !w.config.disable.length && !w.config.enable && !w.config.weekNumbers && /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent); for (u = 0; u < w.config.plugins.length; u++) { var f = w.config.plugins[u](w) || {}; for (var m in f) n.indexOf(m) > -1 ? w.config[m] = c(f[m]).map(E).concat(w.config[m]) : void 0 === i[m] && (w.config[m] = f[m]) } i.altInputClass || (w.config.altInputClass = re().className + " " + w.config.altInputClass); pe("onParseConfig") }(), le(), function () { if (w.input = re(), !w.input) return void w.config.errorHandler(new Error("Invalid input element specified")); w.input._type = w.input.type, w.input.type = "text", w.input.classList.add("flatpickr-input"), w._input = w.input, w.config.altInput && (w.altInput = s(w.input.nodeName, w.config.altInputClass), w._input = w.altInput, w.altInput.placeholder = w.input.placeholder, w.altInput.disabled = w.input.disabled, w.altInput.required = w.input.required, w.altInput.tabIndex = w.input.tabIndex, w.altInput.type = "text", w.input.setAttribute("type", "hidden"), !w.config.static && w.input.parentNode && w.input.parentNode.insertBefore(w.altInput, w.input.nextSibling)); w.config.allowInput || w._input.setAttribute("readonly", "readonly"); w._positionElement = w.config.positionElement || w._input }(), function () { w.selectedDates = [], w.now = w.parseDate(w.config.now) || new Date; var e = w.config.defaultDate || ("INPUT" !== w.input.nodeName && "TEXTAREA" !== w.input.nodeName || !w.input.placeholder || w.input.value !== w.input.placeholder ? w.input.value : null); e && me(e, w.config.dateFormat); w._initialDate = w.selectedDates.length > 0 ? w.selectedDates[0] : w.config.minDate && w.config.minDate.getTime() > w.now.getTime() ? w.config.minDate : w.config.maxDate && w.config.maxDate.getTime() < w.now.getTime() ? w.config.maxDate : w.now, w.currentYear = w._initialDate.getFullYear(), w.currentMonth = w._initialDate.getMonth(), w.selectedDates.length > 0 && (w.latestSelectedDateObj = w.selectedDates[0]); void 0 !== w.config.minTime && (w.config.minTime = w.parseDate(w.config.minTime, "H:i")); void 0 !== w.config.maxTime && (w.config.maxTime = w.parseDate(w.config.maxTime, "H:i")); w.minDateHasTime = !!w.config.minDate && (w.config.minDate.getHours() > 0 || w.config.minDate.getMinutes() > 0 || w.config.minDate.getSeconds() > 0), w.maxDateHasTime = !!w.config.maxDate && (w.config.maxDate.getHours() > 0 || w.config.maxDate.getMinutes() > 0 || w.config.maxDate.getSeconds() > 0) }(), w.utils = { getDaysInMonth: function (e, t) { return void 0 === e && (e = w.currentMonth), void 0 === t && (t = w.currentYear), 1 === e && (t % 4 == 0 && t % 100 != 0 || t % 400 == 0) ? 29 : w.l10n.daysInMonth[e] } }, w.isMobile || function () { var e = window.document.createDocumentFragment(); if (w.calendarContainer = s("div", "flatpickr-calendar"), w.calendarContainer.tabIndex = -1, !w.config.noCalendar) { if (e.appendChild((w.monthNav = s("div", "flatpickr-months"), w.yearElements = [], w.monthElements = [], w.prevMonthNav = s("span", "flatpickr-prev-month"), w.prevMonthNav.innerHTML = w.config.prevArrow, w.nextMonthNav = s("span", "flatpickr-next-month"), w.nextMonthNav.innerHTML = w.config.nextArrow, q(), Object.defineProperty(w, "_hidePrevMonthArrow", { get: function () { return w.__hidePrevMonthArrow }, set: function (e) { w.__hidePrevMonthArrow !== e && (d(w.prevMonthNav, "flatpickr-disabled", e), w.__hidePrevMonthArrow = e) } }), Object.defineProperty(w, "_hideNextMonthArrow", { get: function () { return w.__hideNextMonthArrow }, set: function (e) { w.__hideNextMonthArrow !== e && (d(w.nextMonthNav, "flatpickr-disabled", e), w.__hideNextMonthArrow = e) } }), w.currentYearElement = w.yearElements[0], De(), w.monthNav)), w.innerContainer = s("div", "flatpickr-innerContainer"), w.config.weekNumbers) { var t = function () { w.calendarContainer.classList.add("hasWeeks"); var e = s("div", "flatpickr-weekwrapper"); e.appendChild(s("span", "flatpickr-weekday", w.l10n.weekAbbreviation)); var t = s("div", "flatpickr-weeks"); return e.appendChild(t), { weekWrapper: e, weekNumbers: t } }(), n = t.weekWrapper, a = t.weekNumbers; w.innerContainer.appendChild(n), w.weekNumbers = a, w.weekWrapper = n } w.rContainer = s("div", "flatpickr-rContainer"), w.rContainer.appendChild($()), w.daysContainer || (w.daysContainer = s("div", "flatpickr-days"), w.daysContainer.tabIndex = -1), J(), w.rContainer.appendChild(w.daysContainer), w.innerContainer.appendChild(w.rContainer), e.appendChild(w.innerContainer) } w.config.enableTime && e.appendChild(function () { w.calendarContainer.classList.add("hasTime"), w.config.noCalendar && w.calendarContainer.classList.add("noCalendar"); var e = x(w.config); w.timeContainer = s("div", "flatpickr-time"), w.timeContainer.tabIndex = -1; var t = s("span", "flatpickr-time-separator", ":"), n = m("flatpickr-hour", { "aria-label": w.l10n.hourAriaLabel }); w.hourElement = n.getElementsByTagName("input")[0]; var a = m("flatpickr-minute", { "aria-label": w.l10n.minuteAriaLabel }); w.minuteElement = a.getElementsByTagName("input")[0], w.hourElement.tabIndex = w.minuteElement.tabIndex = -1, w.hourElement.value = o(w.latestSelectedDateObj ? w.latestSelectedDateObj.getHours() : w.config.time_24hr ? e.hours : function (e) { switch (e % 24) { case 0: case 12: return 12; default: return e % 12 } }(e.hours)), w.minuteElement.value = o(w.latestSelectedDateObj ? w.latestSelectedDateObj.getMinutes() : e.minutes), w.hourElement.setAttribute("step", w.config.hourIncrement.toString()), w.minuteElement.setAttribute("step", w.config.minuteIncrement.toString()), w.hourElement.setAttribute("min", w.config.time_24hr ? "0" : "1"), w.hourElement.setAttribute("max", w.config.time_24hr ? "23" : "12"), w.hourElement.setAttribute("maxlength", "2"), w.minuteElement.setAttribute("min", "0"), w.minuteElement.setAttribute("max", "59"), w.minuteElement.setAttribute("maxlength", "2"), w.timeContainer.appendChild(n), w.timeContainer.appendChild(t), w.timeContainer.appendChild(a), w.config.time_24hr && w.timeContainer.classList.add("time24hr"); if (w.config.enableSeconds) { w.timeContainer.classList.add("hasSeconds"); var i = m("flatpickr-second"); w.secondElement = i.getElementsByTagName("input")[0], w.secondElement.value = o(w.latestSelectedDateObj ? w.latestSelectedDateObj.getSeconds() : e.seconds), w.secondElement.setAttribute("step", w.minuteElement.getAttribute("step")), w.secondElement.setAttribute("min", "0"), w.secondElement.setAttribute("max", "59"), w.secondElement.setAttribute("maxlength", "2"), w.timeContainer.appendChild(s("span", "flatpickr-time-separator", ":")), w.timeContainer.appendChild(i) } w.config.time_24hr || (w.amPM = s("span", "flatpickr-am-pm", w.l10n.amPM[r((w.latestSelectedDateObj ? w.hourElement.value : w.config.defaultHour) > 11)]), w.amPM.title = w.l10n.toggleTitle, w.amPM.tabIndex = -1, w.timeContainer.appendChild(w.amPM)); return w.timeContainer }()); d(w.calendarContainer, "rangeMode", "range" === w.config.mode), d(w.calendarContainer, "animate", !0 === w.config.animate), d(w.calendarContainer, "multiMonth", w.config.showMonths > 1), w.calendarContainer.appendChild(e); var i = void 0 !== w.config.appendTo && void 0 !== w.config.appendTo.nodeType; if ((w.config.inline || w.config.static) && (w.calendarContainer.classList.add(w.config.inline ? "inline" : "static"), w.config.inline && (!i && w.element.parentNode ? w.element.parentNode.insertBefore(w.calendarContainer, w._input.nextSibling) : void 0 !== w.config.appendTo && w.config.appendTo.appendChild(w.calendarContainer)), w.config.static)) { var l = s("div", "flatpickr-wrapper"); w.element.parentNode && w.element.parentNode.insertBefore(l, w.element), l.appendChild(w.element), w.altInput && l.appendChild(w.altInput), l.appendChild(w.calendarContainer) } w.config.static || w.config.inline || (void 0 !== w.config.appendTo ? w.config.appendTo : window.document.body).appendChild(w.calendarContainer) }(), function () { w.config.wrap && ["open", "close", "toggle", "clear"].forEach((function (e) { Array.prototype.forEach.call(w.element.querySelectorAll("[data-" + e + "]"), (function (t) { return A(t, "click", w[e]) })) })); if (w.isMobile) return void function () { var e = w.config.enableTime ? w.config.noCalendar ? "time" : "datetime-local" : "date"; w.mobileInput = s("input", w.input.className + " flatpickr-mobile"), w.mobileInput.tabIndex = 1, w.mobileInput.type = e, w.mobileInput.disabled = w.input.disabled, w.mobileInput.required = w.input.required, w.mobileInput.placeholder = w.input.placeholder, w.mobileFormatStr = "datetime-local" === e ? "Y-m-d\\TH:i:S" : "date" === e ? "Y-m-d" : "H:i:S", w.selectedDates.length > 0 && (w.mobileInput.defaultValue = w.mobileInput.value = w.formatDate(w.selectedDates[0], w.mobileFormatStr)); w.config.minDate && (w.mobileInput.min = w.formatDate(w.config.minDate, "Y-m-d")); w.config.maxDate && (w.mobileInput.max = w.formatDate(w.config.maxDate, "Y-m-d")); w.input.getAttribute("step") && (w.mobileInput.step = String(w.input.getAttribute("step"))); w.input.type = "hidden", void 0 !== w.altInput && (w.altInput.type = "hidden"); try { w.input.parentNode && w.input.parentNode.insertBefore(w.mobileInput, w.input.nextSibling) } catch (e) { } A(w.mobileInput, "change", (function (e) { w.setDate(g(e).value, !1, w.mobileFormatStr), pe("onChange"), pe("onClose") })) }(); var e = l(ie, 50); w._debouncedChange = l(N, 300), w.daysContainer && !/iPhone|iPad|iPod/i.test(navigator.userAgent) && A(w.daysContainer, "mouseover", (function (e) { "range" === w.config.mode && ae(g(e)) })); A(window.document.body, "keydown", ne), w.config.inline || w.config.static || A(window, "resize", e); void 0 !== window.ontouchstart ? A(window.document, "touchstart", Z) : A(window.document, "mousedown", Z); A(window.document, "focus", Z, { capture: !0 }), !0 === w.config.clickOpens && (A(w._input, "focus", w.open), A(w._input, "click", w.open)); void 0 !== w.daysContainer && (A(w.monthNav, "click", Ce), A(w.monthNav, ["keyup", "increment"], F), A(w.daysContainer, "click", ue)); if (void 0 !== w.timeContainer && void 0 !== w.minuteElement && void 0 !== w.hourElement) { var t = function (e) { return g(e).select() }; A(w.timeContainer, ["increment"], I), A(w.timeContainer, "blur", I, { capture: !0 }), A(w.timeContainer, "click", Y), A([w.hourElement, w.minuteElement], ["focus", "click"], t), void 0 !== w.secondElement && A(w.secondElement, "focus", (function () { return w.secondElement && w.secondElement.select() })), void 0 !== w.amPM && A(w.amPM, "click", (function (e) { I(e), N() })) } w.config.allowInput && A(w._input, "blur", te) }(), (w.selectedDates.length || w.config.noCalendar) && (w.config.enableTime && _(w.config.noCalendar ? w.latestSelectedDateObj : void 0), be(!1)), k(); var t = /^((?!chrome|android).)*safari/i.test(navigator.userAgent); !w.isMobile && t && ce(), pe("onReady") }(), w } function k(e, t) { for (var n = Array.prototype.slice.call(e).filter((function (e) { return e instanceof HTMLElement })), a = [], i = 0; i < n.length; i++) { var o = n[i]; try { if (null !== o.getAttribute("data-fp-omit")) continue; void 0 !== o._flatpickr && (o._flatpickr.destroy(), o._flatpickr = void 0), o._flatpickr = E(o, t || {}), a.push(o._flatpickr) } catch (e) { console.error(e) } } return 1 === a.length ? a[0] : a } "undefined" != typeof HTMLElement && "undefined" != typeof HTMLCollection && "undefined" != typeof NodeList && (HTMLCollection.prototype.flatpickr = NodeList.prototype.flatpickr = function (e) { return k(this, e) }, HTMLElement.prototype.flatpickr = function (e) { return k([this], e) }); var T = function (e, t) { return "string" == typeof e ? k(window.document.querySelectorAll(e), t) : e instanceof Node ? k([e], t) : k(e, t) }; return T.defaultConfig = {}, T.l10ns = { en: e({}, i), default: e({}, i) }, T.localize = function (t) { T.l10ns.default = e(e({}, T.l10ns.default), t) }, T.setDefaults = function (t) { T.defaultConfig = e(e({}, T.defaultConfig), t) }, T.parseDate = C({}), T.formatDate = b({}), T.compareDates = M, "undefined" != typeof jQuery && void 0 !== jQuery.fn && (jQuery.fn.flatpickr = function (e) { return k(this, e) }), Date.prototype.fp_incr = function (e) { return new Date(this.getFullYear(), this.getMonth(), this.getDate() + ("string" == typeof e ? parseInt(e, 10) : e)) }, "undefined" != typeof window && (window.flatpickr = T), T }));

/* momentjs 2.27.0*/
!function (e, t) { "object" == typeof exports && "undefined" != typeof module ? module.exports = t() : "function" == typeof define && define.amd ? define(t) : e.moment = t() }(this, function () { "use strict"; var e, i; function f() { return e.apply(null, arguments) } function o(e) { return e instanceof Array || "[object Array]" === Object.prototype.toString.call(e) } function u(e) { return null != e && "[object Object]" === Object.prototype.toString.call(e) } function m(e, t) { return Object.prototype.hasOwnProperty.call(e, t) } function l(e) { if (Object.getOwnPropertyNames) return 0 === Object.getOwnPropertyNames(e).length; var t; for (t in e) if (m(e, t)) return; return 1 } function r(e) { return void 0 === e } function h(e) { return "number" == typeof e || "[object Number]" === Object.prototype.toString.call(e) } function a(e) { return e instanceof Date || "[object Date]" === Object.prototype.toString.call(e) } function d(e, t) { for (var n = [], s = 0; s < e.length; ++s)n.push(t(e[s], s)); return n } function c(e, t) { for (var n in t) m(t, n) && (e[n] = t[n]); return m(t, "toString") && (e.toString = t.toString), m(t, "valueOf") && (e.valueOf = t.valueOf), e } function _(e, t, n, s) { return xt(e, t, n, s, !0).utc() } function y(e) { return null == e._pf && (e._pf = { empty: !1, unusedTokens: [], unusedInput: [], overflow: -2, charsLeftOver: 0, nullInput: !1, invalidEra: null, invalidMonth: null, invalidFormat: !1, userInvalidated: !1, iso: !1, parsedDateParts: [], era: null, meridiem: null, rfc2822: !1, weekdayMismatch: !1 }), e._pf } function g(e) { if (null == e._isValid) { var t = y(e), n = i.call(t.parsedDateParts, function (e) { return null != e }), s = !isNaN(e._d.getTime()) && t.overflow < 0 && !t.empty && !t.invalidEra && !t.invalidMonth && !t.invalidWeekday && !t.weekdayMismatch && !t.nullInput && !t.invalidFormat && !t.userInvalidated && (!t.meridiem || t.meridiem && n); if (e._strict && (s = s && 0 === t.charsLeftOver && 0 === t.unusedTokens.length && void 0 === t.bigHour), null != Object.isFrozen && Object.isFrozen(e)) return s; e._isValid = s } return e._isValid } function w(e) { var t = _(NaN); return null != e ? c(y(t), e) : y(t).userInvalidated = !0, t } i = Array.prototype.some ? Array.prototype.some : function (e) { for (var t = Object(this), n = t.length >>> 0, s = 0; s < n; s++)if (s in t && e.call(this, t[s], s, t)) return !0; return !1 }; var p = f.momentProperties = [], t = !1; function v(e, t) { var n, s, i; if (r(t._isAMomentObject) || (e._isAMomentObject = t._isAMomentObject), r(t._i) || (e._i = t._i), r(t._f) || (e._f = t._f), r(t._l) || (e._l = t._l), r(t._strict) || (e._strict = t._strict), r(t._tzm) || (e._tzm = t._tzm), r(t._isUTC) || (e._isUTC = t._isUTC), r(t._offset) || (e._offset = t._offset), r(t._pf) || (e._pf = y(t)), r(t._locale) || (e._locale = t._locale), 0 < p.length) for (n = 0; n < p.length; n++)r(i = t[s = p[n]]) || (e[s] = i); return e } function k(e) { v(this, e), this._d = new Date(null != e._d ? e._d.getTime() : NaN), this.isValid() || (this._d = new Date(NaN)), !1 === t && (t = !0, f.updateOffset(this), t = !1) } function M(e) { return e instanceof k || null != e && null != e._isAMomentObject } function D(e) { !1 === f.suppressDeprecationWarnings && "undefined" != typeof console && console.warn && console.warn("Deprecation warning: " + e) } function n(i, r) { var a = !0; return c(function () { if (null != f.deprecationHandler && f.deprecationHandler(null, i), a) { for (var e, t, n = [], s = 0; s < arguments.length; s++) { if (e = "", "object" == typeof arguments[s]) { for (t in e += "\n[" + s + "] ", arguments[0]) m(arguments[0], t) && (e += t + ": " + arguments[0][t] + ", "); e = e.slice(0, -2) } else e = arguments[s]; n.push(e) } D(i + "\nArguments: " + Array.prototype.slice.call(n).join("") + "\n" + (new Error).stack), a = !1 } return r.apply(this, arguments) }, r) } var s, S = {}; function Y(e, t) { null != f.deprecationHandler && f.deprecationHandler(e, t), S[e] || (D(t), S[e] = !0) } function O(e) { return "undefined" != typeof Function && e instanceof Function || "[object Function]" === Object.prototype.toString.call(e) } function b(e, t) { var n, s = c({}, e); for (n in t) m(t, n) && (u(e[n]) && u(t[n]) ? (s[n] = {}, c(s[n], e[n]), c(s[n], t[n])) : null != t[n] ? s[n] = t[n] : delete s[n]); for (n in e) m(e, n) && !m(t, n) && u(e[n]) && (s[n] = c({}, s[n])); return s } function x(e) { null != e && this.set(e) } f.suppressDeprecationWarnings = !1, f.deprecationHandler = null, s = Object.keys ? Object.keys : function (e) { var t, n = []; for (t in e) m(e, t) && n.push(t); return n }; function T(e, t, n) { var s = "" + Math.abs(e), i = t - s.length; return (0 <= e ? n ? "+" : "" : "-") + Math.pow(10, Math.max(0, i)).toString().substr(1) + s } var N = /(\[[^\[]*\])|(\\)?([Hh]mm(ss)?|Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|Qo?|N{1,5}|YYYYYY|YYYYY|YYYY|YY|y{2,4}|yo?|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|kk?|mm?|ss?|S{1,9}|x|X|zz?|ZZ?|.)/g, P = /(\[[^\[]*\])|(\\)?(LTS|LT|LL?L?L?|l{1,4})/g, R = {}, W = {}; function C(e, t, n, s) { var i = "string" == typeof s ? function () { return this[s]() } : s; e && (W[e] = i), t && (W[t[0]] = function () { return T(i.apply(this, arguments), t[1], t[2]) }), n && (W[n] = function () { return this.localeData().ordinal(i.apply(this, arguments), e) }) } function U(e, t) { return e.isValid() ? (t = H(t, e.localeData()), R[t] = R[t] || function (s) { for (var e, i = s.match(N), t = 0, r = i.length; t < r; t++)W[i[t]] ? i[t] = W[i[t]] : i[t] = (e = i[t]).match(/\[[\s\S]/) ? e.replace(/^\[|\]$/g, "") : e.replace(/\\/g, ""); return function (e) { for (var t = "", n = 0; n < r; n++)t += O(i[n]) ? i[n].call(e, s) : i[n]; return t } }(t), R[t](e)) : e.localeData().invalidDate() } function H(e, t) { var n = 5; function s(e) { return t.longDateFormat(e) || e } for (P.lastIndex = 0; 0 <= n && P.test(e);)e = e.replace(P, s), P.lastIndex = 0, --n; return e } var F = {}; function L(e, t) { var n = e.toLowerCase(); F[n] = F[n + "s"] = F[t] = e } function V(e) { return "string" == typeof e ? F[e] || F[e.toLowerCase()] : void 0 } function G(e) { var t, n, s = {}; for (n in e) m(e, n) && (t = V(n)) && (s[t] = e[n]); return s } var E = {}; function A(e, t) { E[e] = t } function j(e) { return e % 4 == 0 && e % 100 != 0 || e % 400 == 0 } function I(e) { return e < 0 ? Math.ceil(e) || 0 : Math.floor(e) } function Z(e) { var t = +e, n = 0; return 0 != t && isFinite(t) && (n = I(t)), n } function z(t, n) { return function (e) { return null != e ? (q(this, t, e), f.updateOffset(this, n), this) : $(this, t) } } function $(e, t) { return e.isValid() ? e._d["get" + (e._isUTC ? "UTC" : "") + t]() : NaN } function q(e, t, n) { e.isValid() && !isNaN(n) && ("FullYear" === t && j(e.year()) && 1 === e.month() && 29 === e.date() ? (n = Z(n), e._d["set" + (e._isUTC ? "UTC" : "") + t](n, e.month(), xe(n, e.month()))) : e._d["set" + (e._isUTC ? "UTC" : "") + t](n)) } var B, J = /\d/, Q = /\d\d/, X = /\d{3}/, K = /\d{4}/, ee = /[+-]?\d{6}/, te = /\d\d?/, ne = /\d\d\d\d?/, se = /\d\d\d\d\d\d?/, ie = /\d{1,3}/, re = /\d{1,4}/, ae = /[+-]?\d{1,6}/, oe = /\d+/, ue = /[+-]?\d+/, le = /Z|[+-]\d\d:?\d\d/gi, he = /Z|[+-]\d\d(?::?\d\d)?/gi, de = /[0-9]{0,256}['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFF07\uFF10-\uFFEF]{1,256}|[\u0600-\u06FF\/]{1,256}(\s*?[\u0600-\u06FF]{1,256}){1,2}/i; function ce(e, n, s) { B[e] = O(n) ? n : function (e, t) { return e && s ? s : n } } function fe(e, t) { return m(B, e) ? B[e](t._strict, t._locale) : new RegExp(me(e.replace("\\", "").replace(/\\(\[)|\\(\])|\[([^\]\[]*)\]|\\(.)/g, function (e, t, n, s, i) { return t || n || s || i }))) } function me(e) { return e.replace(/[-\/\\^$*+?.()|[\]{}]/g, "\\$&") } B = {}; var _e = {}; function ye(e, n) { var t, s = n; for ("string" == typeof e && (e = [e]), h(n) && (s = function (e, t) { t[n] = Z(e) }), t = 0; t < e.length; t++)_e[e[t]] = s } function ge(e, i) { ye(e, function (e, t, n, s) { n._w = n._w || {}, i(e, n._w, n, s) }) } var we, pe = 0, ve = 1, ke = 2, Me = 3, De = 4, Se = 5, Ye = 6, Oe = 7, be = 8; function xe(e, t) { if (isNaN(e) || isNaN(t)) return NaN; var n, s = (t % (n = 12) + n) % n; return e += (t - s) / 12, 1 == s ? j(e) ? 29 : 28 : 31 - s % 7 % 2 } we = Array.prototype.indexOf ? Array.prototype.indexOf : function (e) { for (var t = 0; t < this.length; ++t)if (this[t] === e) return t; return -1 }, C("M", ["MM", 2], "Mo", function () { return this.month() + 1 }), C("MMM", 0, 0, function (e) { return this.localeData().monthsShort(this, e) }), C("MMMM", 0, 0, function (e) { return this.localeData().months(this, e) }), L("month", "M"), A("month", 8), ce("M", te), ce("MM", te, Q), ce("MMM", function (e, t) { return t.monthsShortRegex(e) }), ce("MMMM", function (e, t) { return t.monthsRegex(e) }), ye(["M", "MM"], function (e, t) { t[ve] = Z(e) - 1 }), ye(["MMM", "MMMM"], function (e, t, n, s) { var i = n._locale.monthsParse(e, s, n._strict); null != i ? t[ve] = i : y(n).invalidMonth = e }); var Te = "January_February_March_April_May_June_July_August_September_October_November_December".split("_"), Ne = "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_"), Pe = /D[oD]?(\[[^\[\]]*\]|\s)+MMMM?/, Re = de, We = de; function Ce(e, t) { var n; if (!e.isValid()) return e; if ("string" == typeof t) if (/^\d+$/.test(t)) t = Z(t); else if (!h(t = e.localeData().monthsParse(t))) return e; return n = Math.min(e.date(), xe(e.year(), t)), e._d["set" + (e._isUTC ? "UTC" : "") + "Month"](t, n), e } function Ue(e) { return null != e ? (Ce(this, e), f.updateOffset(this, !0), this) : $(this, "Month") } function He() { function e(e, t) { return t.length - e.length } for (var t, n = [], s = [], i = [], r = 0; r < 12; r++)t = _([2e3, r]), n.push(this.monthsShort(t, "")), s.push(this.months(t, "")), i.push(this.months(t, "")), i.push(this.monthsShort(t, "")); for (n.sort(e), s.sort(e), i.sort(e), r = 0; r < 12; r++)n[r] = me(n[r]), s[r] = me(s[r]); for (r = 0; r < 24; r++)i[r] = me(i[r]); this._monthsRegex = new RegExp("^(" + i.join("|") + ")", "i"), this._monthsShortRegex = this._monthsRegex, this._monthsStrictRegex = new RegExp("^(" + s.join("|") + ")", "i"), this._monthsShortStrictRegex = new RegExp("^(" + n.join("|") + ")", "i") } function Fe(e) { return j(e) ? 366 : 365 } C("Y", 0, 0, function () { var e = this.year(); return e <= 9999 ? T(e, 4) : "+" + e }), C(0, ["YY", 2], 0, function () { return this.year() % 100 }), C(0, ["YYYY", 4], 0, "year"), C(0, ["YYYYY", 5], 0, "year"), C(0, ["YYYYYY", 6, !0], 0, "year"), L("year", "y"), A("year", 1), ce("Y", ue), ce("YY", te, Q), ce("YYYY", re, K), ce("YYYYY", ae, ee), ce("YYYYYY", ae, ee), ye(["YYYYY", "YYYYYY"], pe), ye("YYYY", function (e, t) { t[pe] = 2 === e.length ? f.parseTwoDigitYear(e) : Z(e) }), ye("YY", function (e, t) { t[pe] = f.parseTwoDigitYear(e) }), ye("Y", function (e, t) { t[pe] = parseInt(e, 10) }), f.parseTwoDigitYear = function (e) { return Z(e) + (68 < Z(e) ? 1900 : 2e3) }; var Le = z("FullYear", !0); function Ve(e) { var t, n; return e < 100 && 0 <= e ? ((n = Array.prototype.slice.call(arguments))[0] = e + 400, t = new Date(Date.UTC.apply(null, n)), isFinite(t.getUTCFullYear()) && t.setUTCFullYear(e)) : t = new Date(Date.UTC.apply(null, arguments)), t } function Ge(e, t, n) { var s = 7 + t - n; return s - (7 + Ve(e, 0, s).getUTCDay() - t) % 7 - 1 } function Ee(e, t, n, s, i) { var r, a = 1 + 7 * (t - 1) + (7 + n - s) % 7 + Ge(e, s, i), o = a <= 0 ? Fe(r = e - 1) + a : a > Fe(e) ? (r = e + 1, a - Fe(e)) : (r = e, a); return { year: r, dayOfYear: o } } function Ae(e, t, n) { var s, i, r = Ge(e.year(), t, n), a = Math.floor((e.dayOfYear() - r - 1) / 7) + 1; return a < 1 ? s = a + je(i = e.year() - 1, t, n) : a > je(e.year(), t, n) ? (s = a - je(e.year(), t, n), i = e.year() + 1) : (i = e.year(), s = a), { week: s, year: i } } function je(e, t, n) { var s = Ge(e, t, n), i = Ge(e + 1, t, n); return (Fe(e) - s + i) / 7 } C("w", ["ww", 2], "wo", "week"), C("W", ["WW", 2], "Wo", "isoWeek"), L("week", "w"), L("isoWeek", "W"), A("week", 5), A("isoWeek", 5), ce("w", te), ce("ww", te, Q), ce("W", te), ce("WW", te, Q), ge(["w", "ww", "W", "WW"], function (e, t, n, s) { t[s.substr(0, 1)] = Z(e) }); function Ie(e, t) { return e.slice(t, 7).concat(e.slice(0, t)) } C("d", 0, "do", "day"), C("dd", 0, 0, function (e) { return this.localeData().weekdaysMin(this, e) }), C("ddd", 0, 0, function (e) { return this.localeData().weekdaysShort(this, e) }), C("dddd", 0, 0, function (e) { return this.localeData().weekdays(this, e) }), C("e", 0, 0, "weekday"), C("E", 0, 0, "isoWeekday"), L("day", "d"), L("weekday", "e"), L("isoWeekday", "E"), A("day", 11), A("weekday", 11), A("isoWeekday", 11), ce("d", te), ce("e", te), ce("E", te), ce("dd", function (e, t) { return t.weekdaysMinRegex(e) }), ce("ddd", function (e, t) { return t.weekdaysShortRegex(e) }), ce("dddd", function (e, t) { return t.weekdaysRegex(e) }), ge(["dd", "ddd", "dddd"], function (e, t, n, s) { var i = n._locale.weekdaysParse(e, s, n._strict); null != i ? t.d = i : y(n).invalidWeekday = e }), ge(["d", "e", "E"], function (e, t, n, s) { t[s] = Z(e) }); var Ze = "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"), ze = "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_"), $e = "Su_Mo_Tu_We_Th_Fr_Sa".split("_"), qe = de, Be = de, Je = de; function Qe() { function e(e, t) { return t.length - e.length } for (var t, n, s, i, r = [], a = [], o = [], u = [], l = 0; l < 7; l++)t = _([2e3, 1]).day(l), n = me(this.weekdaysMin(t, "")), s = me(this.weekdaysShort(t, "")), i = me(this.weekdays(t, "")), r.push(n), a.push(s), o.push(i), u.push(n), u.push(s), u.push(i); r.sort(e), a.sort(e), o.sort(e), u.sort(e), this._weekdaysRegex = new RegExp("^(" + u.join("|") + ")", "i"), this._weekdaysShortRegex = this._weekdaysRegex, this._weekdaysMinRegex = this._weekdaysRegex, this._weekdaysStrictRegex = new RegExp("^(" + o.join("|") + ")", "i"), this._weekdaysShortStrictRegex = new RegExp("^(" + a.join("|") + ")", "i"), this._weekdaysMinStrictRegex = new RegExp("^(" + r.join("|") + ")", "i") } function Xe() { return this.hours() % 12 || 12 } function Ke(e, t) { C(e, 0, 0, function () { return this.localeData().meridiem(this.hours(), this.minutes(), t) }) } function et(e, t) { return t._meridiemParse } C("H", ["HH", 2], 0, "hour"), C("h", ["hh", 2], 0, Xe), C("k", ["kk", 2], 0, function () { return this.hours() || 24 }), C("hmm", 0, 0, function () { return "" + Xe.apply(this) + T(this.minutes(), 2) }), C("hmmss", 0, 0, function () { return "" + Xe.apply(this) + T(this.minutes(), 2) + T(this.seconds(), 2) }), C("Hmm", 0, 0, function () { return "" + this.hours() + T(this.minutes(), 2) }), C("Hmmss", 0, 0, function () { return "" + this.hours() + T(this.minutes(), 2) + T(this.seconds(), 2) }), Ke("a", !0), Ke("A", !1), L("hour", "h"), A("hour", 13), ce("a", et), ce("A", et), ce("H", te), ce("h", te), ce("k", te), ce("HH", te, Q), ce("hh", te, Q), ce("kk", te, Q), ce("hmm", ne), ce("hmmss", se), ce("Hmm", ne), ce("Hmmss", se), ye(["H", "HH"], Me), ye(["k", "kk"], function (e, t, n) { var s = Z(e); t[Me] = 24 === s ? 0 : s }), ye(["a", "A"], function (e, t, n) { n._isPm = n._locale.isPM(e), n._meridiem = e }), ye(["h", "hh"], function (e, t, n) { t[Me] = Z(e), y(n).bigHour = !0 }), ye("hmm", function (e, t, n) { var s = e.length - 2; t[Me] = Z(e.substr(0, s)), t[De] = Z(e.substr(s)), y(n).bigHour = !0 }), ye("hmmss", function (e, t, n) { var s = e.length - 4, i = e.length - 2; t[Me] = Z(e.substr(0, s)), t[De] = Z(e.substr(s, 2)), t[Se] = Z(e.substr(i)), y(n).bigHour = !0 }), ye("Hmm", function (e, t, n) { var s = e.length - 2; t[Me] = Z(e.substr(0, s)), t[De] = Z(e.substr(s)) }), ye("Hmmss", function (e, t, n) { var s = e.length - 4, i = e.length - 2; t[Me] = Z(e.substr(0, s)), t[De] = Z(e.substr(s, 2)), t[Se] = Z(e.substr(i)) }); var tt = z("Hours", !0); var nt, st = { calendar: { sameDay: "[Today at] LT", nextDay: "[Tomorrow at] LT", nextWeek: "dddd [at] LT", lastDay: "[Yesterday at] LT", lastWeek: "[Last] dddd [at] LT", sameElse: "L" }, longDateFormat: { LTS: "h:mm:ss A", LT: "h:mm A", L: "MM/DD/YYYY", LL: "MMMM D, YYYY", LLL: "MMMM D, YYYY h:mm A", LLLL: "dddd, MMMM D, YYYY h:mm A" }, invalidDate: "Invalid date", ordinal: "%d", dayOfMonthOrdinalParse: /\d{1,2}/, relativeTime: { future: "in %s", past: "%s ago", s: "a few seconds", ss: "%d seconds", m: "a minute", mm: "%d minutes", h: "an hour", hh: "%d hours", d: "a day", dd: "%d days", w: "a week", ww: "%d weeks", M: "a month", MM: "%d months", y: "a year", yy: "%d years" }, months: Te, monthsShort: Ne, week: { dow: 0, doy: 6 }, weekdays: Ze, weekdaysMin: $e, weekdaysShort: ze, meridiemParse: /[ap]\.?m?\.?/i }, it = {}, rt = {}; function at(e) { return e ? e.toLowerCase().replace("_", "-") : e } function ot(e) { for (var t, n, s, i, r = 0; r < e.length;) { for (t = (i = at(e[r]).split("-")).length, n = (n = at(e[r + 1])) ? n.split("-") : null; 0 < t;) { if (s = ut(i.slice(0, t).join("-"))) return s; if (n && n.length >= t && function (e, t) { for (var n = Math.min(e.length, t.length), s = 0; s < n; s += 1)if (e[s] !== t[s]) return s; return n }(i, n) >= t - 1) break; t-- } r++ } return nt } function ut(t) { var e = null; if (void 0 === it[t] && "undefined" != typeof module && module && module.exports) try { e = nt._abbr, require("./locale/" + t), lt(e) } catch (e) { it[t] = null } return it[t] } function lt(e, t) { var n; return e && ((n = r(t) ? dt(e) : ht(e, t)) ? nt = n : "undefined" != typeof console && console.warn && console.warn("Locale " + e + " not found. Did you forget to load it?")), nt._abbr } function ht(e, t) { if (null === t) return delete it[e], null; var n, s = st; if (t.abbr = e, null != it[e]) Y("defineLocaleOverride", "use moment.updateLocale(localeName, config) to change an existing locale. moment.defineLocale(localeName, config) should only be used for creating a new locale See http://momentjs.com/guides/#/warnings/define-locale/ for more info."), s = it[e]._config; else if (null != t.parentLocale) if (null != it[t.parentLocale]) s = it[t.parentLocale]._config; else { if (null == (n = ut(t.parentLocale))) return rt[t.parentLocale] || (rt[t.parentLocale] = []), rt[t.parentLocale].push({ name: e, config: t }), null; s = n._config } return it[e] = new x(b(s, t)), rt[e] && rt[e].forEach(function (e) { ht(e.name, e.config) }), lt(e), it[e] } function dt(e) { var t; if (e && e._locale && e._locale._abbr && (e = e._locale._abbr), !e) return nt; if (!o(e)) { if (t = ut(e)) return t; e = [e] } return ot(e) } function ct(e) { var t, n = e._a; return n && -2 === y(e).overflow && (t = n[ve] < 0 || 11 < n[ve] ? ve : n[ke] < 1 || n[ke] > xe(n[pe], n[ve]) ? ke : n[Me] < 0 || 24 < n[Me] || 24 === n[Me] && (0 !== n[De] || 0 !== n[Se] || 0 !== n[Ye]) ? Me : n[De] < 0 || 59 < n[De] ? De : n[Se] < 0 || 59 < n[Se] ? Se : n[Ye] < 0 || 999 < n[Ye] ? Ye : -1, y(e)._overflowDayOfYear && (t < pe || ke < t) && (t = ke), y(e)._overflowWeeks && -1 === t && (t = Oe), y(e)._overflowWeekday && -1 === t && (t = be), y(e).overflow = t), e } var ft = /^\s*((?:[+-]\d{6}|\d{4})-(?:\d\d-\d\d|W\d\d-\d|W\d\d|\d\d\d|\d\d))(?:(T| )(\d\d(?::\d\d(?::\d\d(?:[.,]\d+)?)?)?)([+-]\d\d(?::?\d\d)?|\s*Z)?)?$/, mt = /^\s*((?:[+-]\d{6}|\d{4})(?:\d\d\d\d|W\d\d\d|W\d\d|\d\d\d|\d\d|))(?:(T| )(\d\d(?:\d\d(?:\d\d(?:[.,]\d+)?)?)?)([+-]\d\d(?::?\d\d)?|\s*Z)?)?$/, _t = /Z|[+-]\d\d(?::?\d\d)?/, yt = [["YYYYYY-MM-DD", /[+-]\d{6}-\d\d-\d\d/], ["YYYY-MM-DD", /\d{4}-\d\d-\d\d/], ["GGGG-[W]WW-E", /\d{4}-W\d\d-\d/], ["GGGG-[W]WW", /\d{4}-W\d\d/, !1], ["YYYY-DDD", /\d{4}-\d{3}/], ["YYYY-MM", /\d{4}-\d\d/, !1], ["YYYYYYMMDD", /[+-]\d{10}/], ["YYYYMMDD", /\d{8}/], ["GGGG[W]WWE", /\d{4}W\d{3}/], ["GGGG[W]WW", /\d{4}W\d{2}/, !1], ["YYYYDDD", /\d{7}/], ["YYYYMM", /\d{6}/, !1], ["YYYY", /\d{4}/, !1]], gt = [["HH:mm:ss.SSSS", /\d\d:\d\d:\d\d\.\d+/], ["HH:mm:ss,SSSS", /\d\d:\d\d:\d\d,\d+/], ["HH:mm:ss", /\d\d:\d\d:\d\d/], ["HH:mm", /\d\d:\d\d/], ["HHmmss.SSSS", /\d\d\d\d\d\d\.\d+/], ["HHmmss,SSSS", /\d\d\d\d\d\d,\d+/], ["HHmmss", /\d\d\d\d\d\d/], ["HHmm", /\d\d\d\d/], ["HH", /\d\d/]], wt = /^\/?Date\((-?\d+)/i, pt = /^(?:(Mon|Tue|Wed|Thu|Fri|Sat|Sun),?\s)?(\d{1,2})\s(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s(\d{2,4})\s(\d\d):(\d\d)(?::(\d\d))?\s(?:(UT|GMT|[ECMP][SD]T)|([Zz])|([+-]\d{4}))$/, vt = { UT: 0, GMT: 0, EDT: -240, EST: -300, CDT: -300, CST: -360, MDT: -360, MST: -420, PDT: -420, PST: -480 }; function kt(e) { var t, n, s, i, r, a, o = e._i, u = ft.exec(o) || mt.exec(o); if (u) { for (y(e).iso = !0, t = 0, n = yt.length; t < n; t++)if (yt[t][1].exec(u[1])) { i = yt[t][0], s = !1 !== yt[t][2]; break } if (null == i) return void (e._isValid = !1); if (u[3]) { for (t = 0, n = gt.length; t < n; t++)if (gt[t][1].exec(u[3])) { r = (u[2] || " ") + gt[t][0]; break } if (null == r) return void (e._isValid = !1) } if (!s && null != r) return void (e._isValid = !1); if (u[4]) { if (!_t.exec(u[4])) return void (e._isValid = !1); a = "Z" } e._f = i + (r || "") + (a || ""), Ot(e) } else e._isValid = !1 } function Mt(e, t, n, s, i, r) { var a = [function (e) { var t = parseInt(e, 10); { if (t <= 49) return 2e3 + t; if (t <= 999) return 1900 + t } return t }(e), Ne.indexOf(t), parseInt(n, 10), parseInt(s, 10), parseInt(i, 10)]; return r && a.push(parseInt(r, 10)), a } function Dt(e) { var t, n, s, i, r = pt.exec(e._i.replace(/\([^)]*\)|[\n\t]/g, " ").replace(/(\s\s+)/g, " ").replace(/^\s\s*/, "").replace(/\s\s*$/, "")); if (r) { if (t = Mt(r[4], r[3], r[2], r[5], r[6], r[7]), n = r[1], s = t, i = e, n && ze.indexOf(n) !== new Date(s[0], s[1], s[2]).getDay() && (y(i).weekdayMismatch = !0, !void (i._isValid = !1))) return; e._a = t, e._tzm = function (e, t, n) { if (e) return vt[e]; if (t) return 0; var s = parseInt(n, 10), i = s % 100; return 60 * ((s - i) / 100) + i }(r[8], r[9], r[10]), e._d = Ve.apply(null, e._a), e._d.setUTCMinutes(e._d.getUTCMinutes() - e._tzm), y(e).rfc2822 = !0 } else e._isValid = !1 } function St(e, t, n) { return null != e ? e : null != t ? t : n } function Yt(e) { var t, n, s, i, r, a, o, u = []; if (!e._d) { for (a = e, o = new Date(f.now()), s = a._useUTC ? [o.getUTCFullYear(), o.getUTCMonth(), o.getUTCDate()] : [o.getFullYear(), o.getMonth(), o.getDate()], e._w && null == e._a[ke] && null == e._a[ve] && function (e) { var t, n, s, i, r, a, o, u, l; null != (t = e._w).GG || null != t.W || null != t.E ? (r = 1, a = 4, n = St(t.GG, e._a[pe], Ae(Tt(), 1, 4).year), s = St(t.W, 1), ((i = St(t.E, 1)) < 1 || 7 < i) && (u = !0)) : (r = e._locale._week.dow, a = e._locale._week.doy, l = Ae(Tt(), r, a), n = St(t.gg, e._a[pe], l.year), s = St(t.w, l.week), null != t.d ? ((i = t.d) < 0 || 6 < i) && (u = !0) : null != t.e ? (i = t.e + r, (t.e < 0 || 6 < t.e) && (u = !0)) : i = r); s < 1 || s > je(n, r, a) ? y(e)._overflowWeeks = !0 : null != u ? y(e)._overflowWeekday = !0 : (o = Ee(n, s, i, r, a), e._a[pe] = o.year, e._dayOfYear = o.dayOfYear) }(e), null != e._dayOfYear && (r = St(e._a[pe], s[pe]), (e._dayOfYear > Fe(r) || 0 === e._dayOfYear) && (y(e)._overflowDayOfYear = !0), n = Ve(r, 0, e._dayOfYear), e._a[ve] = n.getUTCMonth(), e._a[ke] = n.getUTCDate()), t = 0; t < 3 && null == e._a[t]; ++t)e._a[t] = u[t] = s[t]; for (; t < 7; t++)e._a[t] = u[t] = null == e._a[t] ? 2 === t ? 1 : 0 : e._a[t]; 24 === e._a[Me] && 0 === e._a[De] && 0 === e._a[Se] && 0 === e._a[Ye] && (e._nextDay = !0, e._a[Me] = 0), e._d = (e._useUTC ? Ve : function (e, t, n, s, i, r, a) { var o; return e < 100 && 0 <= e ? (o = new Date(e + 400, t, n, s, i, r, a), isFinite(o.getFullYear()) && o.setFullYear(e)) : o = new Date(e, t, n, s, i, r, a), o }).apply(null, u), i = e._useUTC ? e._d.getUTCDay() : e._d.getDay(), null != e._tzm && e._d.setUTCMinutes(e._d.getUTCMinutes() - e._tzm), e._nextDay && (e._a[Me] = 24), e._w && void 0 !== e._w.d && e._w.d !== i && (y(e).weekdayMismatch = !0) } } function Ot(e) { if (e._f !== f.ISO_8601) if (e._f !== f.RFC_2822) { e._a = [], y(e).empty = !0; for (var t, n, s, i, r, a, o, u = "" + e._i, l = u.length, h = 0, d = H(e._f, e._locale).match(N) || [], c = 0; c < d.length; c++)n = d[c], (t = (u.match(fe(n, e)) || [])[0]) && (0 < (s = u.substr(0, u.indexOf(t))).length && y(e).unusedInput.push(s), u = u.slice(u.indexOf(t) + t.length), h += t.length), W[n] ? (t ? y(e).empty = !1 : y(e).unusedTokens.push(n), r = n, o = e, null != (a = t) && m(_e, r) && _e[r](a, o._a, o, r)) : e._strict && !t && y(e).unusedTokens.push(n); y(e).charsLeftOver = l - h, 0 < u.length && y(e).unusedInput.push(u), e._a[Me] <= 12 && !0 === y(e).bigHour && 0 < e._a[Me] && (y(e).bigHour = void 0), y(e).parsedDateParts = e._a.slice(0), y(e).meridiem = e._meridiem, e._a[Me] = function (e, t, n) { var s; if (null == n) return t; return null != e.meridiemHour ? e.meridiemHour(t, n) : (null != e.isPM && ((s = e.isPM(n)) && t < 12 && (t += 12), s || 12 !== t || (t = 0)), t) }(e._locale, e._a[Me], e._meridiem), null !== (i = y(e).era) && (e._a[pe] = e._locale.erasConvertYear(i, e._a[pe])), Yt(e), ct(e) } else Dt(e); else kt(e) } function bt(e) { var t, n, s = e._i, i = e._f; return e._locale = e._locale || dt(e._l), null === s || void 0 === i && "" === s ? w({ nullInput: !0 }) : ("string" == typeof s && (e._i = s = e._locale.preparse(s)), M(s) ? new k(ct(s)) : (a(s) ? e._d = s : o(i) ? function (e) { var t, n, s, i, r, a, o = !1; if (0 === e._f.length) return y(e).invalidFormat = !0, e._d = new Date(NaN); for (i = 0; i < e._f.length; i++)r = 0, a = !1, t = v({}, e), null != e._useUTC && (t._useUTC = e._useUTC), t._f = e._f[i], Ot(t), g(t) && (a = !0), r += y(t).charsLeftOver, r += 10 * y(t).unusedTokens.length, y(t).score = r, o ? r < s && (s = r, n = t) : (null == s || r < s || a) && (s = r, n = t, a && (o = !0)); c(e, n || t) }(e) : i ? Ot(e) : r(n = (t = e)._i) ? t._d = new Date(f.now()) : a(n) ? t._d = new Date(n.valueOf()) : "string" == typeof n ? function (e) { var t = wt.exec(e._i); null === t ? (kt(e), !1 === e._isValid && (delete e._isValid, Dt(e), !1 === e._isValid && (delete e._isValid, e._strict ? e._isValid = !1 : f.createFromInputFallback(e)))) : e._d = new Date(+t[1]) }(t) : o(n) ? (t._a = d(n.slice(0), function (e) { return parseInt(e, 10) }), Yt(t)) : u(n) ? function (e) { var t, n; e._d || (n = void 0 === (t = G(e._i)).day ? t.date : t.day, e._a = d([t.year, t.month, n, t.hour, t.minute, t.second, t.millisecond], function (e) { return e && parseInt(e, 10) }), Yt(e)) }(t) : h(n) ? t._d = new Date(n) : f.createFromInputFallback(t), g(e) || (e._d = null), e)) } function xt(e, t, n, s, i) { var r, a = {}; return !0 !== t && !1 !== t || (s = t, t = void 0), !0 !== n && !1 !== n || (s = n, n = void 0), (u(e) && l(e) || o(e) && 0 === e.length) && (e = void 0), a._isAMomentObject = !0, a._useUTC = a._isUTC = i, a._l = n, a._i = e, a._f = t, a._strict = s, (r = new k(ct(bt(a))))._nextDay && (r.add(1, "d"), r._nextDay = void 0), r } function Tt(e, t, n, s) { return xt(e, t, n, s, !1) } f.createFromInputFallback = n("value provided is not in a recognized RFC2822 or ISO format. moment construction falls back to js Date(), which is not reliable across all browsers and versions. Non RFC2822/ISO date formats are discouraged and will be removed in an upcoming major release. Please refer to http://momentjs.com/guides/#/warnings/js-date/ for more info.", function (e) { e._d = new Date(e._i + (e._useUTC ? " UTC" : "")) }), f.ISO_8601 = function () { }, f.RFC_2822 = function () { }; var Nt = n("moment().min is deprecated, use moment.max instead. http://momentjs.com/guides/#/warnings/min-max/", function () { var e = Tt.apply(null, arguments); return this.isValid() && e.isValid() ? e < this ? this : e : w() }), Pt = n("moment().max is deprecated, use moment.min instead. http://momentjs.com/guides/#/warnings/min-max/", function () { var e = Tt.apply(null, arguments); return this.isValid() && e.isValid() ? this < e ? this : e : w() }); function Rt(e, t) { var n, s; if (1 === t.length && o(t[0]) && (t = t[0]), !t.length) return Tt(); for (n = t[0], s = 1; s < t.length; ++s)t[s].isValid() && !t[s][e](n) || (n = t[s]); return n } var Wt = ["year", "quarter", "month", "week", "day", "hour", "minute", "second", "millisecond"]; function Ct(e) { var t = G(e), n = t.year || 0, s = t.quarter || 0, i = t.month || 0, r = t.week || t.isoWeek || 0, a = t.day || 0, o = t.hour || 0, u = t.minute || 0, l = t.second || 0, h = t.millisecond || 0; this._isValid = function (e) { var t, n, s = !1; for (t in e) if (m(e, t) && (-1 === we.call(Wt, t) || null != e[t] && isNaN(e[t]))) return !1; for (n = 0; n < Wt.length; ++n)if (e[Wt[n]]) { if (s) return !1; parseFloat(e[Wt[n]]) !== Z(e[Wt[n]]) && (s = !0) } return !0 }(t), this._milliseconds = +h + 1e3 * l + 6e4 * u + 1e3 * o * 60 * 60, this._days = +a + 7 * r, this._months = +i + 3 * s + 12 * n, this._data = {}, this._locale = dt(), this._bubble() } function Ut(e) { return e instanceof Ct } function Ht(e) { return e < 0 ? -1 * Math.round(-1 * e) : Math.round(e) } function Ft(e, n) { C(e, 0, 0, function () { var e = this.utcOffset(), t = "+"; return e < 0 && (e = -e, t = "-"), t + T(~~(e / 60), 2) + n + T(~~e % 60, 2) }) } Ft("Z", ":"), Ft("ZZ", ""), ce("Z", he), ce("ZZ", he), ye(["Z", "ZZ"], function (e, t, n) { n._useUTC = !0, n._tzm = Vt(he, e) }); var Lt = /([\+\-]|\d\d)/gi; function Vt(e, t) { var n, s, i = (t || "").match(e); return null === i ? null : 0 === (s = 60 * (n = ((i[i.length - 1] || []) + "").match(Lt) || ["-", 0, 0])[1] + Z(n[2])) ? 0 : "+" === n[0] ? s : -s } function Gt(e, t) { var n, s; return t._isUTC ? (n = t.clone(), s = (M(e) || a(e) ? e.valueOf() : Tt(e).valueOf()) - n.valueOf(), n._d.setTime(n._d.valueOf() + s), f.updateOffset(n, !1), n) : Tt(e).local() } function Et(e) { return -Math.round(e._d.getTimezoneOffset()) } function At() { return !!this.isValid() && (this._isUTC && 0 === this._offset) } f.updateOffset = function () { }; var jt = /^(-|\+)?(?:(\d*)[. ])?(\d+):(\d+)(?::(\d+)(\.\d*)?)?$/, It = /^(-|\+)?P(?:([-+]?[0-9,.]*)Y)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)W)?(?:([-+]?[0-9,.]*)D)?(?:T(?:([-+]?[0-9,.]*)H)?(?:([-+]?[0-9,.]*)M)?(?:([-+]?[0-9,.]*)S)?)?$/; function Zt(e, t) { var n, s, i, r = e, a = null; return Ut(e) ? r = { ms: e._milliseconds, d: e._days, M: e._months } : h(e) || !isNaN(+e) ? (r = {}, t ? r[t] = +e : r.milliseconds = +e) : (a = jt.exec(e)) ? (n = "-" === a[1] ? -1 : 1, r = { y: 0, d: Z(a[ke]) * n, h: Z(a[Me]) * n, m: Z(a[De]) * n, s: Z(a[Se]) * n, ms: Z(Ht(1e3 * a[Ye])) * n }) : (a = It.exec(e)) ? (n = "-" === a[1] ? -1 : 1, r = { y: zt(a[2], n), M: zt(a[3], n), w: zt(a[4], n), d: zt(a[5], n), h: zt(a[6], n), m: zt(a[7], n), s: zt(a[8], n) }) : null == r ? r = {} : "object" == typeof r && ("from" in r || "to" in r) && (i = function (e, t) { var n; if (!e.isValid() || !t.isValid()) return { milliseconds: 0, months: 0 }; t = Gt(t, e), e.isBefore(t) ? n = $t(e, t) : ((n = $t(t, e)).milliseconds = -n.milliseconds, n.months = -n.months); return n }(Tt(r.from), Tt(r.to)), (r = {}).ms = i.milliseconds, r.M = i.months), s = new Ct(r), Ut(e) && m(e, "_locale") && (s._locale = e._locale), Ut(e) && m(e, "_isValid") && (s._isValid = e._isValid), s } function zt(e, t) { var n = e && parseFloat(e.replace(",", ".")); return (isNaN(n) ? 0 : n) * t } function $t(e, t) { var n = {}; return n.months = t.month() - e.month() + 12 * (t.year() - e.year()), e.clone().add(n.months, "M").isAfter(t) && --n.months, n.milliseconds = t - e.clone().add(n.months, "M"), n } function qt(s, i) { return function (e, t) { var n; return null === t || isNaN(+t) || (Y(i, "moment()." + i + "(period, number) is deprecated. Please use moment()." + i + "(number, period). See http://momentjs.com/guides/#/warnings/add-inverted-param/ for more info."), n = e, e = t, t = n), Bt(this, Zt(e, t), s), this } } function Bt(e, t, n, s) { var i = t._milliseconds, r = Ht(t._days), a = Ht(t._months); e.isValid() && (s = null == s || s, a && Ce(e, $(e, "Month") + a * n), r && q(e, "Date", $(e, "Date") + r * n), i && e._d.setTime(e._d.valueOf() + i * n), s && f.updateOffset(e, r || a)) } Zt.fn = Ct.prototype, Zt.invalid = function () { return Zt(NaN) }; var Jt = qt(1, "add"), Qt = qt(-1, "subtract"); function Xt(e) { return "string" == typeof e || e instanceof String } function Kt(e) { return M(e) || a(e) || Xt(e) || h(e) || function (t) { var e = o(t), n = !1; e && (n = 0 === t.filter(function (e) { return !h(e) && Xt(t) }).length); return e && n }(e) || function (e) { var t, n, s = u(e) && !l(e), i = !1, r = ["years", "year", "y", "months", "month", "M", "days", "day", "d", "dates", "date", "D", "hours", "hour", "h", "minutes", "minute", "m", "seconds", "second", "s", "milliseconds", "millisecond", "ms"]; for (t = 0; t < r.length; t += 1)n = r[t], i = i || m(e, n); return s && i }(e) || null == e } function en(e, t) { if (e.date() < t.date()) return -en(t, e); var n = 12 * (t.year() - e.year()) + (t.month() - e.month()), s = e.clone().add(n, "months"), i = t - s < 0 ? (t - s) / (s - e.clone().add(n - 1, "months")) : (t - s) / (e.clone().add(1 + n, "months") - s); return -(n + i) || 0 } function tn(e) { var t; return void 0 === e ? this._locale._abbr : (null != (t = dt(e)) && (this._locale = t), this) } f.defaultFormat = "YYYY-MM-DDTHH:mm:ssZ", f.defaultFormatUtc = "YYYY-MM-DDTHH:mm:ss[Z]"; var nn = n("moment().lang() is deprecated. Instead, use moment().localeData() to get the language configuration. Use moment().locale() to change languages.", function (e) { return void 0 === e ? this.localeData() : this.locale(e) }); function sn() { return this._locale } var rn = 126227808e5; function an(e, t) { return (e % t + t) % t } function on(e, t, n) { return e < 100 && 0 <= e ? new Date(e + 400, t, n) - rn : new Date(e, t, n).valueOf() } function un(e, t, n) { return e < 100 && 0 <= e ? Date.UTC(e + 400, t, n) - rn : Date.UTC(e, t, n) } function ln(e, t) { return t.erasAbbrRegex(e) } function hn() { for (var e = [], t = [], n = [], s = [], i = this.eras(), r = 0, a = i.length; r < a; ++r)t.push(me(i[r].name)), e.push(me(i[r].abbr)), n.push(me(i[r].narrow)), s.push(me(i[r].name)), s.push(me(i[r].abbr)), s.push(me(i[r].narrow)); this._erasRegex = new RegExp("^(" + s.join("|") + ")", "i"), this._erasNameRegex = new RegExp("^(" + t.join("|") + ")", "i"), this._erasAbbrRegex = new RegExp("^(" + e.join("|") + ")", "i"), this._erasNarrowRegex = new RegExp("^(" + n.join("|") + ")", "i") } function dn(e, t) { C(0, [e, e.length], 0, t) } function cn(e, t, n, s, i) { var r; return null == e ? Ae(this, s, i).year : ((r = je(e, s, i)) < t && (t = r), function (e, t, n, s, i) { var r = Ee(e, t, n, s, i), a = Ve(r.year, 0, r.dayOfYear); return this.year(a.getUTCFullYear()), this.month(a.getUTCMonth()), this.date(a.getUTCDate()), this }.call(this, e, t, n, s, i)) } C("N", 0, 0, "eraAbbr"), C("NN", 0, 0, "eraAbbr"), C("NNN", 0, 0, "eraAbbr"), C("NNNN", 0, 0, "eraName"), C("NNNNN", 0, 0, "eraNarrow"), C("y", ["y", 1], "yo", "eraYear"), C("y", ["yy", 2], 0, "eraYear"), C("y", ["yyy", 3], 0, "eraYear"), C("y", ["yyyy", 4], 0, "eraYear"), ce("N", ln), ce("NN", ln), ce("NNN", ln), ce("NNNN", function (e, t) { return t.erasNameRegex(e) }), ce("NNNNN", function (e, t) { return t.erasNarrowRegex(e) }), ye(["N", "NN", "NNN", "NNNN", "NNNNN"], function (e, t, n, s) { var i = n._locale.erasParse(e, s, n._strict); i ? y(n).era = i : y(n).invalidEra = e }), ce("y", oe), ce("yy", oe), ce("yyy", oe), ce("yyyy", oe), ce("yo", function (e, t) { return t._eraYearOrdinalRegex || oe }), ye(["y", "yy", "yyy", "yyyy"], pe), ye(["yo"], function (e, t, n, s) { var i; n._locale._eraYearOrdinalRegex && (i = e.match(n._locale._eraYearOrdinalRegex)), n._locale.eraYearOrdinalParse ? t[pe] = n._locale.eraYearOrdinalParse(e, i) : t[pe] = parseInt(e, 10) }), C(0, ["gg", 2], 0, function () { return this.weekYear() % 100 }), C(0, ["GG", 2], 0, function () { return this.isoWeekYear() % 100 }), dn("gggg", "weekYear"), dn("ggggg", "weekYear"), dn("GGGG", "isoWeekYear"), dn("GGGGG", "isoWeekYear"), L("weekYear", "gg"), L("isoWeekYear", "GG"), A("weekYear", 1), A("isoWeekYear", 1), ce("G", ue), ce("g", ue), ce("GG", te, Q), ce("gg", te, Q), ce("GGGG", re, K), ce("gggg", re, K), ce("GGGGG", ae, ee), ce("ggggg", ae, ee), ge(["gggg", "ggggg", "GGGG", "GGGGG"], function (e, t, n, s) { t[s.substr(0, 2)] = Z(e) }), ge(["gg", "GG"], function (e, t, n, s) { t[s] = f.parseTwoDigitYear(e) }), C("Q", 0, "Qo", "quarter"), L("quarter", "Q"), A("quarter", 7), ce("Q", J), ye("Q", function (e, t) { t[ve] = 3 * (Z(e) - 1) }), C("D", ["DD", 2], "Do", "date"), L("date", "D"), A("date", 9), ce("D", te), ce("DD", te, Q), ce("Do", function (e, t) { return e ? t._dayOfMonthOrdinalParse || t._ordinalParse : t._dayOfMonthOrdinalParseLenient }), ye(["D", "DD"], ke), ye("Do", function (e, t) { t[ke] = Z(e.match(te)[0]) }); var fn = z("Date", !0); C("DDD", ["DDDD", 3], "DDDo", "dayOfYear"), L("dayOfYear", "DDD"), A("dayOfYear", 4), ce("DDD", ie), ce("DDDD", X), ye(["DDD", "DDDD"], function (e, t, n) { n._dayOfYear = Z(e) }), C("m", ["mm", 2], 0, "minute"), L("minute", "m"), A("minute", 14), ce("m", te), ce("mm", te, Q), ye(["m", "mm"], De); var mn = z("Minutes", !1); C("s", ["ss", 2], 0, "second"), L("second", "s"), A("second", 15), ce("s", te), ce("ss", te, Q), ye(["s", "ss"], Se); var _n, yn, gn = z("Seconds", !1); for (C("S", 0, 0, function () { return ~~(this.millisecond() / 100) }), C(0, ["SS", 2], 0, function () { return ~~(this.millisecond() / 10) }), C(0, ["SSS", 3], 0, "millisecond"), C(0, ["SSSS", 4], 0, function () { return 10 * this.millisecond() }), C(0, ["SSSSS", 5], 0, function () { return 100 * this.millisecond() }), C(0, ["SSSSSS", 6], 0, function () { return 1e3 * this.millisecond() }), C(0, ["SSSSSSS", 7], 0, function () { return 1e4 * this.millisecond() }), C(0, ["SSSSSSSS", 8], 0, function () { return 1e5 * this.millisecond() }), C(0, ["SSSSSSSSS", 9], 0, function () { return 1e6 * this.millisecond() }), L("millisecond", "ms"), A("millisecond", 16), ce("S", ie, J), ce("SS", ie, Q), ce("SSS", ie, X), _n = "SSSS"; _n.length <= 9; _n += "S")ce(_n, oe); function wn(e, t) { t[Ye] = Z(1e3 * ("0." + e)) } for (_n = "S"; _n.length <= 9; _n += "S")ye(_n, wn); yn = z("Milliseconds", !1), C("z", 0, 0, "zoneAbbr"), C("zz", 0, 0, "zoneName"); var pn = k.prototype; function vn(e) { return e } pn.add = Jt, pn.calendar = function (e, t) { 1 === arguments.length && (Kt(arguments[0]) ? (e = arguments[0], t = void 0) : function (e) { for (var t = u(e) && !l(e), n = !1, s = ["sameDay", "nextDay", "lastDay", "nextWeek", "lastWeek", "sameElse"], i = 0; i < s.length; i += 1)n = n || m(e, s[i]); return t && n }(arguments[0]) && (t = arguments[0], e = void 0)); var n = e || Tt(), s = Gt(n, this).startOf("day"), i = f.calendarFormat(this, s) || "sameElse", r = t && (O(t[i]) ? t[i].call(this, n) : t[i]); return this.format(r || this.localeData().calendar(i, this, Tt(n))) }, pn.clone = function () { return new k(this) }, pn.diff = function (e, t, n) { var s, i, r; if (!this.isValid()) return NaN; if (!(s = Gt(e, this)).isValid()) return NaN; switch (i = 6e4 * (s.utcOffset() - this.utcOffset()), t = V(t)) { case "year": r = en(this, s) / 12; break; case "month": r = en(this, s); break; case "quarter": r = en(this, s) / 3; break; case "second": r = (this - s) / 1e3; break; case "minute": r = (this - s) / 6e4; break; case "hour": r = (this - s) / 36e5; break; case "day": r = (this - s - i) / 864e5; break; case "week": r = (this - s - i) / 6048e5; break; default: r = this - s }return n ? r : I(r) }, pn.endOf = function (e) { var t, n; if (void 0 === (e = V(e)) || "millisecond" === e || !this.isValid()) return this; switch (n = this._isUTC ? un : on, e) { case "year": t = n(this.year() + 1, 0, 1) - 1; break; case "quarter": t = n(this.year(), this.month() - this.month() % 3 + 3, 1) - 1; break; case "month": t = n(this.year(), this.month() + 1, 1) - 1; break; case "week": t = n(this.year(), this.month(), this.date() - this.weekday() + 7) - 1; break; case "isoWeek": t = n(this.year(), this.month(), this.date() - (this.isoWeekday() - 1) + 7) - 1; break; case "day": case "date": t = n(this.year(), this.month(), this.date() + 1) - 1; break; case "hour": t = this._d.valueOf(), t += 36e5 - an(t + (this._isUTC ? 0 : 6e4 * this.utcOffset()), 36e5) - 1; break; case "minute": t = this._d.valueOf(), t += 6e4 - an(t, 6e4) - 1; break; case "second": t = this._d.valueOf(), t += 1e3 - an(t, 1e3) - 1; break }return this._d.setTime(t), f.updateOffset(this, !0), this }, pn.format = function (e) { e = e || (this.isUtc() ? f.defaultFormatUtc : f.defaultFormat); var t = U(this, e); return this.localeData().postformat(t) }, pn.from = function (e, t) { return this.isValid() && (M(e) && e.isValid() || Tt(e).isValid()) ? Zt({ to: this, from: e }).locale(this.locale()).humanize(!t) : this.localeData().invalidDate() }, pn.fromNow = function (e) { return this.from(Tt(), e) }, pn.to = function (e, t) { return this.isValid() && (M(e) && e.isValid() || Tt(e).isValid()) ? Zt({ from: this, to: e }).locale(this.locale()).humanize(!t) : this.localeData().invalidDate() }, pn.toNow = function (e) { return this.to(Tt(), e) }, pn.get = function (e) { return O(this[e = V(e)]) ? this[e]() : this }, pn.invalidAt = function () { return y(this).overflow }, pn.isAfter = function (e, t) { var n = M(e) ? e : Tt(e); return !(!this.isValid() || !n.isValid()) && ("millisecond" === (t = V(t) || "millisecond") ? this.valueOf() > n.valueOf() : n.valueOf() < this.clone().startOf(t).valueOf()) }, pn.isBefore = function (e, t) { var n = M(e) ? e : Tt(e); return !(!this.isValid() || !n.isValid()) && ("millisecond" === (t = V(t) || "millisecond") ? this.valueOf() < n.valueOf() : this.clone().endOf(t).valueOf() < n.valueOf()) }, pn.isBetween = function (e, t, n, s) { var i = M(e) ? e : Tt(e), r = M(t) ? t : Tt(t); return !!(this.isValid() && i.isValid() && r.isValid()) && (("(" === (s = s || "()")[0] ? this.isAfter(i, n) : !this.isBefore(i, n)) && (")" === s[1] ? this.isBefore(r, n) : !this.isAfter(r, n))) }, pn.isSame = function (e, t) { var n, s = M(e) ? e : Tt(e); return !(!this.isValid() || !s.isValid()) && ("millisecond" === (t = V(t) || "millisecond") ? this.valueOf() === s.valueOf() : (n = s.valueOf(), this.clone().startOf(t).valueOf() <= n && n <= this.clone().endOf(t).valueOf())) }, pn.isSameOrAfter = function (e, t) { return this.isSame(e, t) || this.isAfter(e, t) }, pn.isSameOrBefore = function (e, t) { return this.isSame(e, t) || this.isBefore(e, t) }, pn.isValid = function () { return g(this) }, pn.lang = nn, pn.locale = tn, pn.localeData = sn, pn.max = Pt, pn.min = Nt, pn.parsingFlags = function () { return c({}, y(this)) }, pn.set = function (e, t) { if ("object" == typeof e) for (var n = function (e) { var t, n = []; for (t in e) m(e, t) && n.push({ unit: t, priority: E[t] }); return n.sort(function (e, t) { return e.priority - t.priority }), n }(e = G(e)), s = 0; s < n.length; s++)this[n[s].unit](e[n[s].unit]); else if (O(this[e = V(e)])) return this[e](t); return this }, pn.startOf = function (e) { var t, n; if (void 0 === (e = V(e)) || "millisecond" === e || !this.isValid()) return this; switch (n = this._isUTC ? un : on, e) { case "year": t = n(this.year(), 0, 1); break; case "quarter": t = n(this.year(), this.month() - this.month() % 3, 1); break; case "month": t = n(this.year(), this.month(), 1); break; case "week": t = n(this.year(), this.month(), this.date() - this.weekday()); break; case "isoWeek": t = n(this.year(), this.month(), this.date() - (this.isoWeekday() - 1)); break; case "day": case "date": t = n(this.year(), this.month(), this.date()); break; case "hour": t = this._d.valueOf(), t -= an(t + (this._isUTC ? 0 : 6e4 * this.utcOffset()), 36e5); break; case "minute": t = this._d.valueOf(), t -= an(t, 6e4); break; case "second": t = this._d.valueOf(), t -= an(t, 1e3); break }return this._d.setTime(t), f.updateOffset(this, !0), this }, pn.subtract = Qt, pn.toArray = function () { var e = this; return [e.year(), e.month(), e.date(), e.hour(), e.minute(), e.second(), e.millisecond()] }, pn.toObject = function () { var e = this; return { years: e.year(), months: e.month(), date: e.date(), hours: e.hours(), minutes: e.minutes(), seconds: e.seconds(), milliseconds: e.milliseconds() } }, pn.toDate = function () { return new Date(this.valueOf()) }, pn.toISOString = function (e) { if (!this.isValid()) return null; var t = !0 !== e, n = t ? this.clone().utc() : this; return n.year() < 0 || 9999 < n.year() ? U(n, t ? "YYYYYY-MM-DD[T]HH:mm:ss.SSS[Z]" : "YYYYYY-MM-DD[T]HH:mm:ss.SSSZ") : O(Date.prototype.toISOString) ? t ? this.toDate().toISOString() : new Date(this.valueOf() + 60 * this.utcOffset() * 1e3).toISOString().replace("Z", U(n, "Z")) : U(n, t ? "YYYY-MM-DD[T]HH:mm:ss.SSS[Z]" : "YYYY-MM-DD[T]HH:mm:ss.SSSZ") }, pn.inspect = function () { if (!this.isValid()) return "moment.invalid(/* " + this._i + " */)"; var e, t, n, s = "moment", i = ""; return this.isLocal() || (s = 0 === this.utcOffset() ? "moment.utc" : "moment.parseZone", i = "Z"), e = "[" + s + '("]', t = 0 <= this.year() && this.year() <= 9999 ? "YYYY" : "YYYYYY", n = i + '[")]', this.format(e + t + "-MM-DD[T]HH:mm:ss.SSS" + n) }, "undefined" != typeof Symbol && null != Symbol.for && (pn[Symbol.for("nodejs.util.inspect.custom")] = function () { return "Moment<" + this.format() + ">" }), pn.toJSON = function () { return this.isValid() ? this.toISOString() : null }, pn.toString = function () { return this.clone().locale("en").format("ddd MMM DD YYYY HH:mm:ss [GMT]ZZ") }, pn.unix = function () { return Math.floor(this.valueOf() / 1e3) }, pn.valueOf = function () { return this._d.valueOf() - 6e4 * (this._offset || 0) }, pn.creationData = function () { return { input: this._i, format: this._f, locale: this._locale, isUTC: this._isUTC, strict: this._strict } }, pn.eraName = function () { for (var e, t = this.localeData().eras(), n = 0, s = t.length; n < s; ++n) { if (e = this.startOf("day").valueOf(), t[n].since <= e && e <= t[n].until) return t[n].name; if (t[n].until <= e && e <= t[n].since) return t[n].name } return "" }, pn.eraNarrow = function () { for (var e, t = this.localeData().eras(), n = 0, s = t.length; n < s; ++n) { if (e = this.startOf("day").valueOf(), t[n].since <= e && e <= t[n].until) return t[n].narrow; if (t[n].until <= e && e <= t[n].since) return t[n].narrow } return "" }, pn.eraAbbr = function () { for (var e, t = this.localeData().eras(), n = 0, s = t.length; n < s; ++n) { if (e = this.startOf("day").valueOf(), t[n].since <= e && e <= t[n].until) return t[n].abbr; if (t[n].until <= e && e <= t[n].since) return t[n].abbr } return "" }, pn.eraYear = function () { for (var e, t, n = this.localeData().eras(), s = 0, i = n.length; s < i; ++s)if (e = n[s].since <= n[s].until ? 1 : -1, t = this.startOf("day").valueOf(), n[s].since <= t && t <= n[s].until || n[s].until <= t && t <= n[s].since) return (this.year() - f(n[s].since).year()) * e + n[s].offset; return this.year() }, pn.year = Le, pn.isLeapYear = function () { return j(this.year()) }, pn.weekYear = function (e) { return cn.call(this, e, this.week(), this.weekday(), this.localeData()._week.dow, this.localeData()._week.doy) }, pn.isoWeekYear = function (e) { return cn.call(this, e, this.isoWeek(), this.isoWeekday(), 1, 4) }, pn.quarter = pn.quarters = function (e) { return null == e ? Math.ceil((this.month() + 1) / 3) : this.month(3 * (e - 1) + this.month() % 3) }, pn.month = Ue, pn.daysInMonth = function () { return xe(this.year(), this.month()) }, pn.week = pn.weeks = function (e) { var t = this.localeData().week(this); return null == e ? t : this.add(7 * (e - t), "d") }, pn.isoWeek = pn.isoWeeks = function (e) { var t = Ae(this, 1, 4).week; return null == e ? t : this.add(7 * (e - t), "d") }, pn.weeksInYear = function () { var e = this.localeData()._week; return je(this.year(), e.dow, e.doy) }, pn.weeksInWeekYear = function () { var e = this.localeData()._week; return je(this.weekYear(), e.dow, e.doy) }, pn.isoWeeksInYear = function () { return je(this.year(), 1, 4) }, pn.isoWeeksInISOWeekYear = function () { return je(this.isoWeekYear(), 1, 4) }, pn.date = fn, pn.day = pn.days = function (e) { if (!this.isValid()) return null != e ? this : NaN; var t, n, s = this._isUTC ? this._d.getUTCDay() : this._d.getDay(); return null != e ? (t = e, n = this.localeData(), e = "string" != typeof t ? t : isNaN(t) ? "number" == typeof (t = n.weekdaysParse(t)) ? t : null : parseInt(t, 10), this.add(e - s, "d")) : s }, pn.weekday = function (e) { if (!this.isValid()) return null != e ? this : NaN; var t = (this.day() + 7 - this.localeData()._week.dow) % 7; return null == e ? t : this.add(e - t, "d") }, pn.isoWeekday = function (e) { if (!this.isValid()) return null != e ? this : NaN; if (null == e) return this.day() || 7; var t, n, s = (t = e, n = this.localeData(), "string" == typeof t ? n.weekdaysParse(t) % 7 || 7 : isNaN(t) ? null : t); return this.day(this.day() % 7 ? s : s - 7) }, pn.dayOfYear = function (e) { var t = Math.round((this.clone().startOf("day") - this.clone().startOf("year")) / 864e5) + 1; return null == e ? t : this.add(e - t, "d") }, pn.hour = pn.hours = tt, pn.minute = pn.minutes = mn, pn.second = pn.seconds = gn, pn.millisecond = pn.milliseconds = yn, pn.utcOffset = function (e, t, n) { var s, i = this._offset || 0; if (!this.isValid()) return null != e ? this : NaN; if (null == e) return this._isUTC ? i : Et(this); if ("string" == typeof e) { if (null === (e = Vt(he, e))) return this } else Math.abs(e) < 16 && !n && (e *= 60); return !this._isUTC && t && (s = Et(this)), this._offset = e, this._isUTC = !0, null != s && this.add(s, "m"), i !== e && (!t || this._changeInProgress ? Bt(this, Zt(e - i, "m"), 1, !1) : this._changeInProgress || (this._changeInProgress = !0, f.updateOffset(this, !0), this._changeInProgress = null)), this }, pn.utc = function (e) { return this.utcOffset(0, e) }, pn.local = function (e) { return this._isUTC && (this.utcOffset(0, e), this._isUTC = !1, e && this.subtract(Et(this), "m")), this }, pn.parseZone = function () { var e; return null != this._tzm ? this.utcOffset(this._tzm, !1, !0) : "string" == typeof this._i && (null != (e = Vt(le, this._i)) ? this.utcOffset(e) : this.utcOffset(0, !0)), this }, pn.hasAlignedHourOffset = function (e) { return !!this.isValid() && (e = e ? Tt(e).utcOffset() : 0, (this.utcOffset() - e) % 60 == 0) }, pn.isDST = function () { return this.utcOffset() > this.clone().month(0).utcOffset() || this.utcOffset() > this.clone().month(5).utcOffset() }, pn.isLocal = function () { return !!this.isValid() && !this._isUTC }, pn.isUtcOffset = function () { return !!this.isValid() && this._isUTC }, pn.isUtc = At, pn.isUTC = At, pn.zoneAbbr = function () { return this._isUTC ? "UTC" : "" }, pn.zoneName = function () { return this._isUTC ? "Coordinated Universal Time" : "" }, pn.dates = n("dates accessor is deprecated. Use date instead.", fn), pn.months = n("months accessor is deprecated. Use month instead", Ue), pn.years = n("years accessor is deprecated. Use year instead", Le), pn.zone = n("moment().zone is deprecated, use moment().utcOffset instead. http://momentjs.com/guides/#/warnings/zone/", function (e, t) { return null != e ? ("string" != typeof e && (e = -e), this.utcOffset(e, t), this) : -this.utcOffset() }), pn.isDSTShifted = n("isDSTShifted is deprecated. See http://momentjs.com/guides/#/warnings/dst-shifted/ for more information", function () { if (!r(this._isDSTShifted)) return this._isDSTShifted; var e, t = {}; return v(t, this), (t = bt(t))._a ? (e = (t._isUTC ? _ : Tt)(t._a), this._isDSTShifted = this.isValid() && 0 < function (e, t, n) { for (var s = Math.min(e.length, t.length), i = Math.abs(e.length - t.length), r = 0, a = 0; a < s; a++)(n && e[a] !== t[a] || !n && Z(e[a]) !== Z(t[a])) && r++; return r + i }(t._a, e.toArray())) : this._isDSTShifted = !1, this._isDSTShifted }); var kn = x.prototype; function Mn(e, t, n, s) { var i = dt(), r = _().set(s, t); return i[n](r, e) } function Dn(e, t, n) { if (h(e) && (t = e, e = void 0), e = e || "", null != t) return Mn(e, t, n, "month"); for (var s = [], i = 0; i < 12; i++)s[i] = Mn(e, i, n, "month"); return s } function Sn(e, t, n, s) { t = ("boolean" == typeof e ? h(t) && (n = t, t = void 0) : (t = e, e = !1, h(n = t) && (n = t, t = void 0)), t || ""); var i, r = dt(), a = e ? r._week.dow : 0, o = []; if (null != n) return Mn(t, (n + a) % 7, s, "day"); for (i = 0; i < 7; i++)o[i] = Mn(t, (i + a) % 7, s, "day"); return o } kn.calendar = function (e, t, n) { var s = this._calendar[e] || this._calendar.sameElse; return O(s) ? s.call(t, n) : s }, kn.longDateFormat = function (e) { var t = this._longDateFormat[e], n = this._longDateFormat[e.toUpperCase()]; return t || !n ? t : (this._longDateFormat[e] = n.match(N).map(function (e) { return "MMMM" === e || "MM" === e || "DD" === e || "dddd" === e ? e.slice(1) : e }).join(""), this._longDateFormat[e]) }, kn.invalidDate = function () { return this._invalidDate }, kn.ordinal = function (e) { return this._ordinal.replace("%d", e) }, kn.preparse = vn, kn.postformat = vn, kn.relativeTime = function (e, t, n, s) { var i = this._relativeTime[n]; return O(i) ? i(e, t, n, s) : i.replace(/%d/i, e) }, kn.pastFuture = function (e, t) { var n = this._relativeTime[0 < e ? "future" : "past"]; return O(n) ? n(t) : n.replace(/%s/i, t) }, kn.set = function (e) { var t, n; for (n in e) m(e, n) && (O(t = e[n]) ? this[n] = t : this["_" + n] = t); this._config = e, this._dayOfMonthOrdinalParseLenient = new RegExp((this._dayOfMonthOrdinalParse.source || this._ordinalParse.source) + "|" + /\d{1,2}/.source) }, kn.eras = function (e, t) { for (var n, s = this._eras || dt("en")._eras, i = 0, r = s.length; i < r; ++i) { switch (typeof s[i].since) { case "string": n = f(s[i].since).startOf("day"), s[i].since = n.valueOf(); break }switch (typeof s[i].until) { case "undefined": s[i].until = 1 / 0; break; case "string": n = f(s[i].until).startOf("day").valueOf(), s[i].until = n.valueOf(); break } } return s }, kn.erasParse = function (e, t, n) { var s, i, r, a, o, u = this.eras(); for (e = e.toUpperCase(), s = 0, i = u.length; s < i; ++s)if (r = u[s].name.toUpperCase(), a = u[s].abbr.toUpperCase(), o = u[s].narrow.toUpperCase(), n) switch (t) { case "N": case "NN": case "NNN": if (a === e) return u[s]; break; case "NNNN": if (r === e) return u[s]; break; case "NNNNN": if (o === e) return u[s]; break } else if (0 <= [r, a, o].indexOf(e)) return u[s] }, kn.erasConvertYear = function (e, t) { var n = e.since <= e.until ? 1 : -1; return void 0 === t ? f(e.since).year() : f(e.since).year() + (t - e.offset) * n }, kn.erasAbbrRegex = function (e) { return m(this, "_erasAbbrRegex") || hn.call(this), e ? this._erasAbbrRegex : this._erasRegex }, kn.erasNameRegex = function (e) { return m(this, "_erasNameRegex") || hn.call(this), e ? this._erasNameRegex : this._erasRegex }, kn.erasNarrowRegex = function (e) { return m(this, "_erasNarrowRegex") || hn.call(this), e ? this._erasNarrowRegex : this._erasRegex }, kn.months = function (e, t) { return e ? o(this._months) ? this._months[e.month()] : this._months[(this._months.isFormat || Pe).test(t) ? "format" : "standalone"][e.month()] : o(this._months) ? this._months : this._months.standalone }, kn.monthsShort = function (e, t) { return e ? o(this._monthsShort) ? this._monthsShort[e.month()] : this._monthsShort[Pe.test(t) ? "format" : "standalone"][e.month()] : o(this._monthsShort) ? this._monthsShort : this._monthsShort.standalone }, kn.monthsParse = function (e, t, n) { var s, i, r; if (this._monthsParseExact) return function (e, t, n) { var s, i, r, a = e.toLocaleLowerCase(); if (!this._monthsParse) for (this._monthsParse = [], this._longMonthsParse = [], this._shortMonthsParse = [], s = 0; s < 12; ++s)r = _([2e3, s]), this._shortMonthsParse[s] = this.monthsShort(r, "").toLocaleLowerCase(), this._longMonthsParse[s] = this.months(r, "").toLocaleLowerCase(); return n ? "MMM" === t ? -1 !== (i = we.call(this._shortMonthsParse, a)) ? i : null : -1 !== (i = we.call(this._longMonthsParse, a)) ? i : null : "MMM" === t ? -1 !== (i = we.call(this._shortMonthsParse, a)) || -1 !== (i = we.call(this._longMonthsParse, a)) ? i : null : -1 !== (i = we.call(this._longMonthsParse, a)) || -1 !== (i = we.call(this._shortMonthsParse, a)) ? i : null }.call(this, e, t, n); for (this._monthsParse || (this._monthsParse = [], this._longMonthsParse = [], this._shortMonthsParse = []), s = 0; s < 12; s++) { if (i = _([2e3, s]), n && !this._longMonthsParse[s] && (this._longMonthsParse[s] = new RegExp("^" + this.months(i, "").replace(".", "") + "$", "i"), this._shortMonthsParse[s] = new RegExp("^" + this.monthsShort(i, "").replace(".", "") + "$", "i")), n || this._monthsParse[s] || (r = "^" + this.months(i, "") + "|^" + this.monthsShort(i, ""), this._monthsParse[s] = new RegExp(r.replace(".", ""), "i")), n && "MMMM" === t && this._longMonthsParse[s].test(e)) return s; if (n && "MMM" === t && this._shortMonthsParse[s].test(e)) return s; if (!n && this._monthsParse[s].test(e)) return s } }, kn.monthsRegex = function (e) { return this._monthsParseExact ? (m(this, "_monthsRegex") || He.call(this), e ? this._monthsStrictRegex : this._monthsRegex) : (m(this, "_monthsRegex") || (this._monthsRegex = We), this._monthsStrictRegex && e ? this._monthsStrictRegex : this._monthsRegex) }, kn.monthsShortRegex = function (e) { return this._monthsParseExact ? (m(this, "_monthsRegex") || He.call(this), e ? this._monthsShortStrictRegex : this._monthsShortRegex) : (m(this, "_monthsShortRegex") || (this._monthsShortRegex = Re), this._monthsShortStrictRegex && e ? this._monthsShortStrictRegex : this._monthsShortRegex) }, kn.week = function (e) { return Ae(e, this._week.dow, this._week.doy).week }, kn.firstDayOfYear = function () { return this._week.doy }, kn.firstDayOfWeek = function () { return this._week.dow }, kn.weekdays = function (e, t) { var n = o(this._weekdays) ? this._weekdays : this._weekdays[e && !0 !== e && this._weekdays.isFormat.test(t) ? "format" : "standalone"]; return !0 === e ? Ie(n, this._week.dow) : e ? n[e.day()] : n }, kn.weekdaysMin = function (e) { return !0 === e ? Ie(this._weekdaysMin, this._week.dow) : e ? this._weekdaysMin[e.day()] : this._weekdaysMin }, kn.weekdaysShort = function (e) { return !0 === e ? Ie(this._weekdaysShort, this._week.dow) : e ? this._weekdaysShort[e.day()] : this._weekdaysShort }, kn.weekdaysParse = function (e, t, n) { var s, i, r; if (this._weekdaysParseExact) return function (e, t, n) { var s, i, r, a = e.toLocaleLowerCase(); if (!this._weekdaysParse) for (this._weekdaysParse = [], this._shortWeekdaysParse = [], this._minWeekdaysParse = [], s = 0; s < 7; ++s)r = _([2e3, 1]).day(s), this._minWeekdaysParse[s] = this.weekdaysMin(r, "").toLocaleLowerCase(), this._shortWeekdaysParse[s] = this.weekdaysShort(r, "").toLocaleLowerCase(), this._weekdaysParse[s] = this.weekdays(r, "").toLocaleLowerCase(); return n ? "dddd" === t ? -1 !== (i = we.call(this._weekdaysParse, a)) ? i : null : "ddd" === t ? -1 !== (i = we.call(this._shortWeekdaysParse, a)) ? i : null : -1 !== (i = we.call(this._minWeekdaysParse, a)) ? i : null : "dddd" === t ? -1 !== (i = we.call(this._weekdaysParse, a)) || -1 !== (i = we.call(this._shortWeekdaysParse, a)) || -1 !== (i = we.call(this._minWeekdaysParse, a)) ? i : null : "ddd" === t ? -1 !== (i = we.call(this._shortWeekdaysParse, a)) || -1 !== (i = we.call(this._weekdaysParse, a)) || -1 !== (i = we.call(this._minWeekdaysParse, a)) ? i : null : -1 !== (i = we.call(this._minWeekdaysParse, a)) || -1 !== (i = we.call(this._weekdaysParse, a)) || -1 !== (i = we.call(this._shortWeekdaysParse, a)) ? i : null }.call(this, e, t, n); for (this._weekdaysParse || (this._weekdaysParse = [], this._minWeekdaysParse = [], this._shortWeekdaysParse = [], this._fullWeekdaysParse = []), s = 0; s < 7; s++) { if (i = _([2e3, 1]).day(s), n && !this._fullWeekdaysParse[s] && (this._fullWeekdaysParse[s] = new RegExp("^" + this.weekdays(i, "").replace(".", "\\.?") + "$", "i"), this._shortWeekdaysParse[s] = new RegExp("^" + this.weekdaysShort(i, "").replace(".", "\\.?") + "$", "i"), this._minWeekdaysParse[s] = new RegExp("^" + this.weekdaysMin(i, "").replace(".", "\\.?") + "$", "i")), this._weekdaysParse[s] || (r = "^" + this.weekdays(i, "") + "|^" + this.weekdaysShort(i, "") + "|^" + this.weekdaysMin(i, ""), this._weekdaysParse[s] = new RegExp(r.replace(".", ""), "i")), n && "dddd" === t && this._fullWeekdaysParse[s].test(e)) return s; if (n && "ddd" === t && this._shortWeekdaysParse[s].test(e)) return s; if (n && "dd" === t && this._minWeekdaysParse[s].test(e)) return s; if (!n && this._weekdaysParse[s].test(e)) return s } }, kn.weekdaysRegex = function (e) { return this._weekdaysParseExact ? (m(this, "_weekdaysRegex") || Qe.call(this), e ? this._weekdaysStrictRegex : this._weekdaysRegex) : (m(this, "_weekdaysRegex") || (this._weekdaysRegex = qe), this._weekdaysStrictRegex && e ? this._weekdaysStrictRegex : this._weekdaysRegex) }, kn.weekdaysShortRegex = function (e) { return this._weekdaysParseExact ? (m(this, "_weekdaysRegex") || Qe.call(this), e ? this._weekdaysShortStrictRegex : this._weekdaysShortRegex) : (m(this, "_weekdaysShortRegex") || (this._weekdaysShortRegex = Be), this._weekdaysShortStrictRegex && e ? this._weekdaysShortStrictRegex : this._weekdaysShortRegex) }, kn.weekdaysMinRegex = function (e) { return this._weekdaysParseExact ? (m(this, "_weekdaysRegex") || Qe.call(this), e ? this._weekdaysMinStrictRegex : this._weekdaysMinRegex) : (m(this, "_weekdaysMinRegex") || (this._weekdaysMinRegex = Je), this._weekdaysMinStrictRegex && e ? this._weekdaysMinStrictRegex : this._weekdaysMinRegex) }, kn.isPM = function (e) { return "p" === (e + "").toLowerCase().charAt(0) }, kn.meridiem = function (e, t, n) { return 11 < e ? n ? "pm" : "PM" : n ? "am" : "AM" }, lt("en", { eras: [{ since: "0001-01-01", until: 1 / 0, offset: 1, name: "Anno Domini", narrow: "AD", abbr: "AD" }, { since: "0000-12-31", until: -1 / 0, offset: 1, name: "Before Christ", narrow: "BC", abbr: "BC" }], dayOfMonthOrdinalParse: /\d{1,2}(th|st|nd|rd)/, ordinal: function (e) { var t = e % 10; return e + (1 === Z(e % 100 / 10) ? "th" : 1 == t ? "st" : 2 == t ? "nd" : 3 == t ? "rd" : "th") } }), f.lang = n("moment.lang is deprecated. Use moment.locale instead.", lt), f.langData = n("moment.langData is deprecated. Use moment.localeData instead.", dt); var Yn = Math.abs; function On(e, t, n, s) { var i = Zt(t, n); return e._milliseconds += s * i._milliseconds, e._days += s * i._days, e._months += s * i._months, e._bubble() } function bn(e) { return e < 0 ? Math.floor(e) : Math.ceil(e) } function xn(e) { return 4800 * e / 146097 } function Tn(e) { return 146097 * e / 4800 } function Nn(e) { return function () { return this.as(e) } } var Pn = Nn("ms"), Rn = Nn("s"), Wn = Nn("m"), Cn = Nn("h"), Un = Nn("d"), Hn = Nn("w"), Fn = Nn("M"), Ln = Nn("Q"), Vn = Nn("y"); function Gn(e) { return function () { return this.isValid() ? this._data[e] : NaN } } var En = Gn("milliseconds"), An = Gn("seconds"), jn = Gn("minutes"), In = Gn("hours"), Zn = Gn("days"), zn = Gn("months"), $n = Gn("years"); var qn = Math.round, Bn = { ss: 44, s: 45, m: 45, h: 22, d: 26, w: null, M: 11 }; function Jn(e, t, n, s) { var i = Zt(e).abs(), r = qn(i.as("s")), a = qn(i.as("m")), o = qn(i.as("h")), u = qn(i.as("d")), l = qn(i.as("M")), h = qn(i.as("w")), d = qn(i.as("y")), c = (r <= n.ss ? ["s", r] : r < n.s && ["ss", r]) || a <= 1 && ["m"] || a < n.m && ["mm", a] || o <= 1 && ["h"] || o < n.h && ["hh", o] || u <= 1 && ["d"] || u < n.d && ["dd", u]; return null != n.w && (c = c || h <= 1 && ["w"] || h < n.w && ["ww", h]), (c = c || l <= 1 && ["M"] || l < n.M && ["MM", l] || d <= 1 && ["y"] || ["yy", d])[2] = t, c[3] = 0 < +e, c[4] = s, function (e, t, n, s, i) { return i.relativeTime(t || 1, !!n, e, s) }.apply(null, c) } var Qn = Math.abs; function Xn(e) { return (0 < e) - (e < 0) || +e } function Kn() { if (!this.isValid()) return this.localeData().invalidDate(); var e, t, n, s, i, r, a, o, u = Qn(this._milliseconds) / 1e3, l = Qn(this._days), h = Qn(this._months), d = this.asSeconds(); return d ? (e = I(u / 60), t = I(e / 60), u %= 60, e %= 60, n = I(h / 12), h %= 12, s = u ? u.toFixed(3).replace(/\.?0+$/, "") : "", i = d < 0 ? "-" : "", r = Xn(this._months) !== Xn(d) ? "-" : "", a = Xn(this._days) !== Xn(d) ? "-" : "", o = Xn(this._milliseconds) !== Xn(d) ? "-" : "", i + "P" + (n ? r + n + "Y" : "") + (h ? r + h + "M" : "") + (l ? a + l + "D" : "") + (t || e || u ? "T" : "") + (t ? o + t + "H" : "") + (e ? o + e + "M" : "") + (u ? o + s + "S" : "")) : "P0D" } var es = Ct.prototype; return es.isValid = function () { return this._isValid }, es.abs = function () { var e = this._data; return this._milliseconds = Yn(this._milliseconds), this._days = Yn(this._days), this._months = Yn(this._months), e.milliseconds = Yn(e.milliseconds), e.seconds = Yn(e.seconds), e.minutes = Yn(e.minutes), e.hours = Yn(e.hours), e.months = Yn(e.months), e.years = Yn(e.years), this }, es.add = function (e, t) { return On(this, e, t, 1) }, es.subtract = function (e, t) { return On(this, e, t, -1) }, es.as = function (e) { if (!this.isValid()) return NaN; var t, n, s = this._milliseconds; if ("month" === (e = V(e)) || "quarter" === e || "year" === e) switch (t = this._days + s / 864e5, n = this._months + xn(t), e) { case "month": return n; case "quarter": return n / 3; case "year": return n / 12 } else switch (t = this._days + Math.round(Tn(this._months)), e) { case "week": return t / 7 + s / 6048e5; case "day": return t + s / 864e5; case "hour": return 24 * t + s / 36e5; case "minute": return 1440 * t + s / 6e4; case "second": return 86400 * t + s / 1e3; case "millisecond": return Math.floor(864e5 * t) + s; default: throw new Error("Unknown unit " + e) } }, es.asMilliseconds = Pn, es.asSeconds = Rn, es.asMinutes = Wn, es.asHours = Cn, es.asDays = Un, es.asWeeks = Hn, es.asMonths = Fn, es.asQuarters = Ln, es.asYears = Vn, es.valueOf = function () { return this.isValid() ? this._milliseconds + 864e5 * this._days + this._months % 12 * 2592e6 + 31536e6 * Z(this._months / 12) : NaN }, es._bubble = function () { var e, t, n, s, i, r = this._milliseconds, a = this._days, o = this._months, u = this._data; return 0 <= r && 0 <= a && 0 <= o || r <= 0 && a <= 0 && o <= 0 || (r += 864e5 * bn(Tn(o) + a), o = a = 0), u.milliseconds = r % 1e3, e = I(r / 1e3), u.seconds = e % 60, t = I(e / 60), u.minutes = t % 60, n = I(t / 60), u.hours = n % 24, a += I(n / 24), o += i = I(xn(a)), a -= bn(Tn(i)), s = I(o / 12), o %= 12, u.days = a, u.months = o, u.years = s, this }, es.clone = function () { return Zt(this) }, es.get = function (e) { return e = V(e), this.isValid() ? this[e + "s"]() : NaN }, es.milliseconds = En, es.seconds = An, es.minutes = jn, es.hours = In, es.days = Zn, es.weeks = function () { return I(this.days() / 7) }, es.months = zn, es.years = $n, es.humanize = function (e, t) { if (!this.isValid()) return this.localeData().invalidDate(); var n, s, i = !1, r = Bn; return "object" == typeof e && (t = e, e = !1), "boolean" == typeof e && (i = e), "object" == typeof t && (r = Object.assign({}, Bn, t), null != t.s && null == t.ss && (r.ss = t.s - 1)), n = this.localeData(), s = Jn(this, !i, r, n), i && (s = n.pastFuture(+this, s)), n.postformat(s) }, es.toISOString = Kn, es.toString = Kn, es.toJSON = Kn, es.locale = tn, es.localeData = sn, es.toIsoString = n("toIsoString() is deprecated. Please use toISOString() instead (notice the capitals)", Kn), es.lang = nn, C("X", 0, 0, "unix"), C("x", 0, 0, "valueOf"), ce("x", ue), ce("X", /[+-]?\d+(\.\d{1,3})?/), ye("X", function (e, t, n) { n._d = new Date(1e3 * parseFloat(e)) }), ye("x", function (e, t, n) { n._d = new Date(Z(e)) }), f.version = "2.27.0", e = Tt, f.fn = pn, f.min = function () { return Rt("isBefore", [].slice.call(arguments, 0)) }, f.max = function () { return Rt("isAfter", [].slice.call(arguments, 0)) }, f.now = function () { return Date.now ? Date.now() : +new Date }, f.utc = _, f.unix = function (e) { return Tt(1e3 * e) }, f.months = function (e, t) { return Dn(e, t, "months") }, f.isDate = a, f.locale = lt, f.invalid = w, f.duration = Zt, f.isMoment = M, f.weekdays = function (e, t, n) { return Sn(e, t, n, "weekdays") }, f.parseZone = function () { return Tt.apply(null, arguments).parseZone() }, f.localeData = dt, f.isDuration = Ut, f.monthsShort = function (e, t) { return Dn(e, t, "monthsShort") }, f.weekdaysMin = function (e, t, n) { return Sn(e, t, n, "weekdaysMin") }, f.defineLocale = ht, f.updateLocale = function (e, t) { var n, s, i; return null != t ? (i = st, null != it[e] && null != it[e].parentLocale ? it[e].set(b(it[e]._config, t)) : (null != (s = ut(e)) && (i = s._config), t = b(i, t), null == s && (t.abbr = e), (n = new x(t)).parentLocale = it[e], it[e] = n), lt(e)) : null != it[e] && (null != it[e].parentLocale ? (it[e] = it[e].parentLocale, e === lt() && lt(e)) : null != it[e] && delete it[e]), it[e] }, f.locales = function () { return s(it) }, f.weekdaysShort = function (e, t, n) { return Sn(e, t, n, "weekdaysShort") }, f.normalizeUnits = V, f.relativeTimeRounding = function (e) { return void 0 === e ? qn : "function" == typeof e && (qn = e, !0) }, f.relativeTimeThreshold = function (e, t) { return void 0 !== Bn[e] && (void 0 === t ? Bn[e] : (Bn[e] = t, "s" === e && (Bn.ss = t - 1), !0)) }, f.calendarFormat = function (e, t) { var n = e.diff(t, "days", !0); return n < -6 ? "sameElse" : n < -1 ? "lastWeek" : n < 0 ? "lastDay" : n < 1 ? "sameDay" : n < 2 ? "nextDay" : n < 7 ? "nextWeek" : "sameElse" }, f.prototype = pn, f.HTML5_FMT = { DATETIME_LOCAL: "YYYY-MM-DDTHH:mm", DATETIME_LOCAL_SECONDS: "YYYY-MM-DDTHH:mm:ss", DATETIME_LOCAL_MS: "YYYY-MM-DDTHH:mm:ss.SSS", DATE: "YYYY-MM-DD", TIME: "HH:mm", TIME_SECONDS: "HH:mm:ss", TIME_MS: "HH:mm:ss.SSS", WEEK: "GGGG-[W]WW", MONTH: "YYYY-MM" }, f });

