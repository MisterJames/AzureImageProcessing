module AzureApplicationDemo.Pages.Upload {
    export class Index {
        filesToUpload = [];
        Init() {
            var zone = document.getElementById("zone");
            zone.ondrop = (event) => {
                event.preventDefault();
                this._readFiles(event.dataTransfer.files);
                document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
            }
            zone.ondragover = function (event) {
                document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-in-text");
                return false;
            };
            zone.ondragleave = function (event) {
                document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
                return false;
            };
            document.getElementById("zonetext").innerHTML = document.getElementById("zonetext").getAttribute("data-out-text");
            document.getElementById("uploadButton").onclick = () => this._uploadFiles();
        }

        _readFiles(files) {
            for (var file of files) {
                this._previewFile(file);
                this.filesToUpload.push(file);
            }
        }

        _uploadFiles() {
            let formData = new FormData();
            for (var fileToUpload of this.filesToUpload) {
                formData.append('files', fileToUpload);
            }
            var xhr = new XMLHttpRequest();
            xhr.open("POST", "/Upload/Send");
            xhr.onprogress = function (event) {
                if (event.lengthComputable)
                    console.log(event.loaded / event.total * 100 | 0);
            }
            xhr.onreadystatechange = () => {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    this.filesToUpload = [];
                    document.getElementById("uploaded").innerHTML = "";
                }
            }
            xhr.send(formData);
        }

        _previewFile(file) {
            var reader = new FileReader();
            reader.onload = function (event: any) {
                var image = new Image();
                image.src = event.target.result;
                image.width = 200;
                var wrapper = document.createElement("div");
                wrapper.classList.add("col-md-3");
                wrapper.appendChild(image);

                document.getElementById("uploaded").appendChild(wrapper);
                

            }
            reader.readAsDataURL(file);
        }


    }
}





