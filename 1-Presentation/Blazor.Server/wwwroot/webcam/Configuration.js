function ready() {
    if (document.readyState == 'complete') {
        Webcam.set({
            width: 320,
            height: 240,
            dest_width: 1280,
            dest_height: 720,
            image_format: 'jpeg',
            jpeg_quality: 90,
            //flip_horiz: true //mirrored 左右反转
        });
        try {
            Webcam.attach('#my_camera');
        } catch (e) {
            //alert(e);
            alert("请检查摄像头是否被其他程序占用。");
        }
    }
}

function take_snapshot() {
    var data = null;
    try {
        Webcam.snap(function (data_uri) {
            data = data_uri;
        });
    } catch (e) {
        return null;
    }
    return data;
}
