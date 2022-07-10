$(document).ready(function () {
    console.log(GetDateToday())
    $('#dateLeave').daterangepicker({

        "minDate": GetDateToday()
    }, function (start, end, label) {
        StartDate = start.format('YYYY-MM-DD')
        console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
    });


    const idEmp = $("#login-employee-id").val()
    console.log("idemp :"+idEmp)
    $.ajax({
        url: `https://localhost:44302/api/employee/${idEmp}/withManager`
    }).done(e => {
        $("#sisa-cuti").text(e.sisaCuti);
        $("#manager-name").text(e.manager.managerName);
        $("#nama-divisi").text(e.divisi.namaDivisi);
    })
})

$("#fileBukti").change(function () {
    let namaFile = document.querySelector('#fileBukti').files[0];
    $("#fileBuktiLabel").html(namaFile.name)
});

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
    const DateReqLeave = document.querySelector("#dateLeave").value
    const [startDate, endDate] = SpliteDate(DateReqLeave)
    console.log(startDate)
    console.log(endDate)
    
    

    var obj = {
        "employee_id": $("#login-employee-id").val(),
        "category_id": parseInt($("#category_id").val()),
        "startDate": startDate,
        "endDate": endDate,
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
                $("#fileBuktiLabel").html("Choose File")
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: result.message,
                })
                document.getElementById("inputLeave").className = "needs-validation";
                break;
            default:
                $("#fileBuktiLabel").html("Choose File")
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

// validate file
function validateSize(input) {
    var redInput = `.form-control:focus {
                      border-color: #red;
                      box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075), 0 0 8px rgba(255, 0, 0, 0.6);
                    }`
    const fileSize = input.files[0].size / 1024 / 1024; // in MiB
    //console.log(getExtFile(input.files[0].name))
    const fileExt = getExtFile(input.files[0].name)
    console.log(fileExt)
    switch (fileExt) {
        case "none":
            break;
        case "jpg":
            break;
        case "pdf":
            break;
        case "png":
            break;
        case "dock":
            break;
        case "doc":
            break;
        case "tiff":
            break;
        case "ppt":
            break;
        case "pptx":
            break;
        case "xlsx":
            break;
        case "xls":
            break;
        case "htm":
            break;
        case "mp3":
            break;
        case "mp4":
            break;
        case "txt":
            break;
        default:
            Swal.fire({
                icon: 'error',
                title: 'Fail',
                text: getExtFile(input.files[0].name)+" file not alowed",
            })
            //$("#fileBuktiLabel").val('')
            document.getElementById("fileBukti").value = "";
            break;
    }

    if (fileSize > 2) {
        Swal.fire({
            icon: 'error',
            title: 'Fail',
            text: "File cannot be more than 2 mb",
        })
        document.getElementById("fileBukti").value = "";
        // $(file).val(''); //for clearing with Jquery
        /*const fileExt = getExtFile(input.files[0].name)
        console.log(fileExt)*/
    } else {
        // Proceed further
    }
}