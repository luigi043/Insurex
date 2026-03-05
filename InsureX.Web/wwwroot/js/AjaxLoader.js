// AjaxLoader.js - Common AJAX loading indicator
var AjaxLoader = {
    show: function () {
        if (!document.getElementById('ajaxLoader')) {
            var overlay = document.createElement('div');
            overlay.id = 'ajaxLoader';
            overlay.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.3);z-index:9999;display:flex;align-items:center;justify-content:center';
            overlay.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div>';
            document.body.appendChild(overlay);
        }
    },
    hide: function () {
        var loader = document.getElementById('ajaxLoader');
        if (loader) loader.remove();
    }
};

// Attach to jQuery globally
$(document).ajaxStart(AjaxLoader.show).ajaxStop(AjaxLoader.hide);
