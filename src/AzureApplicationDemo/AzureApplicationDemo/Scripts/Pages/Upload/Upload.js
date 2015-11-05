var AzureApplicationDemo;
(function (AzureApplicationDemo) {
    var Pages;
    (function (Pages) {
        var Upload;
        (function (Upload) {
            var Index = (function () {
                function Index() {
                    this.filesToUpload = [];
                }
                Index.prototype.Init = function () {
                    var _this = this;
                    var zone = document.getElementById("zone");
                    zone.ondrop = function (event) {
                        event.preventDefault();
                        _this._readFiles(event.dataTransfer.files);
                        document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
                    };
                    zone.ondragover = function (event) {
                        document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-in-text");
                        return false;
                    };
                    zone.ondragleave = function (event) {
                        document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
                        return false;
                    };
                    document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
                    document.getElementById("uploadButton").onclick = function () { return _this._uploadFiles(); };
                };
                Index.prototype._readFiles = function (files) {
                    for (var _i = 0; _i < files.length; _i++) {
                        var file = files[_i];
                        this._previewFile(file);
                        this.filesToUpload.push(file);
                    }
                };
                Index.prototype._uploadFiles = function () {
                    var _this = this;
                    var formData = new FormData();
                    for (var _i = 0, _a = this.filesToUpload; _i < _a.length; _i++) {
                        var fileToUpload = _a[_i];
                        formData.append('files', fileToUpload);
                    }
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", "/Upload/Send");
                    xhr.onprogress = function (event) {
                        if (event.lengthComputable)
                            console.log(event.loaded / event.total * 100 | 0);
                    };
                    xhr.onreadystatechange = function () {
                        if (xhr.readyState == 4 && xhr.status == 200) {
                            _this.filesToUpload = [];
                            document.getElementById("uploaded").innerHTML = "";
                        }
                    };
                    xhr.send(formData);
                };
                Index.prototype._previewFile = function (file) {
                    var reader = new FileReader();
                    reader.onload = function (event) {
                        var image = new Image();
                        image.src = event.target.result;
                        image.width = 200;
                        var wrapper = document.createElement("div");
                        wrapper.classList.add("col-md-3");
                        wrapper.appendChild(image);
                        document.getElementById("uploaded").appendChild(wrapper);
                    };
                    reader.readAsDataURL(file);
                };
                return Index;
            })();
            Upload.Index = Index;
        })(Upload = Pages.Upload || (Pages.Upload = {}));
    })(Pages = AzureApplicationDemo.Pages || (AzureApplicationDemo.Pages = {}));
})(AzureApplicationDemo || (AzureApplicationDemo = {}));
//# sourceMappingURL=Upload.js.map