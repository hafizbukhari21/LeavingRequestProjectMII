
function dateStringToDate(inputDate) {
    let date = new Date(inputDate)
    let m = date.getMonth() + 1
    if (m < 10) m = "0" + m
    let d = date.getDate()
    if (d < 10) d = "0" + d
    let y = date.getFullYear()
    return `${y}-${m}-${d}`
   
}



const toBase64 = file => new Promise((resolve, reject) => {

    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result.replace("data:", "").replace(/^.+,/, ""));
    reader.onerror = error => "";
});

const getExtFile = filename => (/[.]/.exec(filename))[0] ? /[^.]+$/.exec(filename)[0] : undefined


function ValidateForm(forms, actionApi) {
    var validation = Array.prototype.filter.call(forms, function (form) {
        
        form.addEventListener('submit', function (event) {
            alert("sdsd")
            
            event.preventDefault();
            event.stopPropagation();
            if (form.checkValidity() === false) {

            }
            else {

                actionApi()

            }
            form.classList.add('was-validated');
        }, false);
    });
}