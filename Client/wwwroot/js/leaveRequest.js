$(document).ready(function () {
    const idEmp = $("#login-employee-id").val()
    $.ajax({
        url: `https://localhost:44302/api/employee/${idEmp}/withManager`
    }).done(e => {
        $("#sisa-cuti").text(e.sisaCuti);
        $("#manager-name").text(e.manager.managerName);
        $("#nama-divisi").text(e.divisi.namaDivisi);
    })
})

function formReset() {
    document.getElementById("inputLeave").reset();
}
$('#start_date').datepicker({
    onSelect: function (dateText, inst) {
        $('#end_date').datepicker('option', 'minDate', new Date(dateText));
    },
});

$('#end_date').datepicker({
    onSelect: function (dateText, inst) {
        $('#start_date').datepicker('option', 'maxDate', new Date(dateText));
    }
});

$(document).ready(() => {
    let inputLeaveForm = document.getElementsByClassName("inputLeaveForm");
    GetCategory()
    ValidateForm(inputLeaveForm, SubmitFormRequest)
});

function GetCategory() {
    $.ajax({
        url: "https://localhost:44302/api/category"
    }).done(e => {
        e.forEach(elem => {
            $("#category_id").append(`<option value="${elem.category_id}">${elem.nameCategory}</option>`)
        })
    })
}

async function SubmitFormRequest() {
    
    const fileBukti = document.querySelector('#fileBukti').files[0];
    const fileBuktiExt = getExtFile(fileBukti.name)

    var obj = {
        "employee_id": $("#login-employee-id").val(),
        "category_id": parseInt($("#category_id").val()),
        "startDate": dateStringToDate($("#start_date").val()),
        "endDate": dateStringToDate($("#end_date").val()),
        "leavingMessage": $("#leavingMessage").val(),
        "tipeFileBukti": fileBuktiExt,
        "fileBukti": await toBase64(fileBukti),
        "namaFileBukti": fileBukti.name
    }

    console.log(obj)
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(obj),
    }).done((result) => {
        console.log(result)
        switch (result.errorType) {
            case 200:
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: result.message,
                })
                break;
            default :
                Swal.fire({
                    icon: 'error',
                    title: 'Fail',
                    text: result.message,
                })
                break;
        }
        
        formReset();
    }).fail((error) => {
        console.log(error)
    })
}