
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
function SpliteDate(dateLeave) {
    let date = dateLeave.replace(/\s/g, '').split("-",2)
    startDate = dateStringToDate(date[0])
    endDate = dateStringToDate(date[1])
    return [startDate, endDate]
}

function GetDateToday() {
    let today = new Date();
    let dd = String(today.getDate()).padStart(2, '0');
    let mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    let yyyy = today.getFullYear();

    return `${mm}/${dd}/${yyyy};`
}

function SwallSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: message,
        timer: 1500
    })
}

function SwallFail(message) {
    Swal.fire({
        icon: 'error',
        title: 'Fail',
        text: message,
    })
}