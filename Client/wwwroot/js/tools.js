
function dateStringToDate(inputDate) {
    let date = new Date(inputDate)
    let m = date.getMonth() + 1
    if (m < 10) m = "0" + m
    let d = date.getDate()
    if (d < 10) d = "0" + d
    let y = date.getFullYear()
    return `${y}-${m}-${d}`
   
}

//function fileToB64(file, callback) {
//    var file = file['files'][0];

//    var reader = new FileReader();
//    let base64String
//    let imageBase64Stringsep
//    reader.onload = function () {
//        base64String = reader.result.replace("data:", "")
//            .replace(/^.+,/, "");

//        /*imageBase64Stringsep = base64String;*/
//        callback(base64String)
//    }
//    reader.readAsDataURL(file);



//}


const toBase64 = file => new Promise((resolve, reject) => {

    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result.replace("data:", "").replace(/^.+,/, ""));
    reader.onerror = error => "";
});

const getExtFile = filename => (/[.]/.exec(filename))[0] ? /[^.]+$/.exec(filename)[0] : undefined