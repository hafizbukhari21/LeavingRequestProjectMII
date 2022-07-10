const idEmp = $("#login-employee-id").val()

console.log(idEmp)


$(document).ready(function () {
    let formUpdate = document.getElementsByClassName("updateLeaveForm")
    ValidateForm(formUpdate, UpdateRequest)
    //triggerSpesificDataFromSessionStorage()
    GetCategory();
    let table = $('#dataTbl').DataTable({

        ajax: {
            url: `https://localhost:44302/api/leavingrequest/emp/${idEmp}`,
            "dataType": "json",
            "dataSrc": "",
        },
        "order": [[0, "desc"]],
        dom: 'lBfrtip',
        buttons: [
            //'copy', 'csv', 'excel', 'pdf', 'print'
            {
                extend: 'copy',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-copy"> </i>',
                titleAttr: 'Copy to clipboard'
            },
            {
                extend: 'excel',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-excel"> </i>',
                titleAttr: 'Download to excel'
            },
            {
                extend: 'pdf',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-file-pdf"> </i>',
                titleAttr: 'Download to pdf'
            },
            {
                extend: 'print',
                className: 'btn btn-success btn-sm',
                text: '<i class="fas fa-print"> </i>',
                titleAttr: 'Print this table'
            }
        ],
        initComplete: function () {
            var btns = $('.dt-button');
            //btns.addClass('btn btn-primary btn-sm');
            btns.removeClass('dt-button');
            triggerSpesificDataFromSessionStorage()
        },
        
        columns: [
            {
                "data": "requestTime",
                visible: false,
            },
            {
                "data": "employeeName"
            },
            {
                "data": "categoryName"
            },
            {
                "data": "approvalStatus",
                "render": function (data, type, row, meta) {
                    let badge = ""
                    switch (data) {
                        case "Menunggu":
                            badge = "primary"
                            break;
                        case "Ditolak":
                            badge = "danger"
                            break
                        case "Diterima":
                            badge = "success"
                            break;
                        case "Revisi":
                            badge = "warning"
                            break;
                        case "Cancel":
                            badge = "secondary"
                            break;
                    }
                    return `<span class="badge badge-${badge}">${data}</span>`
                }
            },
            {
                data: "request_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <a type="button" class="btn btn-sm btn-warning" id="${data}" title="Delete" onClick="leaveDetail('${data}')" data-toggle="modal" data-target="#updateModal">
                                <i class="fas fa-edit"></i>
                                </a>
                            </div>`
                }
            }
        ]
    });
    
        
});

function triggerSpesificDataFromSessionStorage() {
    
    let requestId = sessionStorage.getItem("request_id")
    document.querySelector("#" + requestId).click()
    
    sessionStorage.removeItem("request_id")
}

$("#btnCancel").click(e => {
    let request_id = $("#request_id").val()
    alert(request_id)
    $.ajax({
        url: `https://localhost:44302/api/leavingrequest/emp/cancel/${request_id}`
    }).done(e => {
        switch (e.errorType) {
            case 200:
                SwallSuccess(e.message)
                leaveDetail(request_id)
                break
            default:
                SwallFail(e.message)
                break
        }
    })
    $("#dataTbl").ajax.reload()
})

function leaveDetail(reqId) {
    let data = {
        request_id: reqId
    }
    console.log(data)
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/emp/detail",
        data: JSON.stringify(data),
        type: "POST",
        contentType: 'application/json',
    }).done(u => {
        $("#namaFileBukti").val(u.namaFileBukti);
        $("#tipeFileBukti").val(u.tipeFileBukti);
        $("#employee_id").val(u.employee_id);

        $("#request_id").val(u.request_id);
        document.getElementById("category_id").value = u.category_id;
        $("#start_date").val(moment(u.startDate).format("MM/DD/yyyy").toString());
        $("#end_date").val(moment(u.endDate).format("MM/DD/yyyy").toString());
        $("#approvalStatusName").val(u.approvalStatusName);
        $("#leavingMessage").val(u.leavingMessage);
        $("#approvalMessage").val(u.approvalMessage);
        $("#downloadFileBukti").attr("href", "data:application/octet-stream;base64," + u.fileBukti);
        $("#downloadFileBukti").attr("download", u.namaFileBukti);
        console.log(moment(u.startDate).format("YYYY-MM-DD").toString())

        $('#dateLeave').daterangepicker({
            "startDate": moment(u.startDate).format("MM/DD/YYYY").toString(),
            "endDate": moment(u.endDate).format("MM/DD/YYYY").toString(),
            "minDate": GetDateToday()
        }, function (start, end, label) {
            StartDate = start.format('YYYY-MM-DD')
            console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
        });

        let modalUpdateFooter = document.querySelector("#modalFooterUpdate")
        if (u.approvalStatusName == "Diterima" || u.approvalStatusName == "Ditolak" || u.approvalStatusName == "Cancel") {
            modalUpdateFooter.style.display = "none";
        }
        else {
            modalUpdateFooter.style.display = "flex";
        }
    })
}

async function UpdateRequest() {
    const DateReqLeave = document.querySelector("#dateLeave").value
    const [startDate, endDate] = SpliteDate(DateReqLeave)
    let fileBukti = null
    let uploadFileUpdate = document.querySelector("#newfileBukti")
    let namaFileBukti = $("#namaFileBukti").val()
    let fileBuktiExt = $("#tipeFileBukti").val()

    if (!uploadFileUpdate.files.length == 0) {
        uploadFileUpdate = document.querySelector("#newfileBukti").files[0]
        fileBuktiExt = getExtFile(uploadFileUpdate.name)
        namaFileBukti = uploadFileUpdate.name
        fileBukti = await toBase64(uploadFileUpdate)
    }

    let obj = {
        "request_id": $("#request_id").val(),
        "employee_id": $("#employee_id").val(),
        "category_id": $("#category_id").val(),
        "startDate": startDate,
        "endDate": endDate,
        "leavingMessage": $("#leavingMessage").val(),

        "tipeFileBukti": fileBuktiExt,
        "namaFileBukti": namaFileBukti,
        "fileBukti": fileBukti
    }

    
    //obj.fileBukti = $("#fileBukti").val();
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
        //data: obj, //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        if (result.errorType == 200) {
            SwallSuccess(result.message)
        }
        else SwallFail(result.message)
        console.log(result);
    }).fail((error) => {
        console.log(error)
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
            footer: '<a href="">Why do I have this issue?</a>'
        })
    })
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


function GetCategory() {
    $.ajax({
        url: "https://localhost:44302/api/category"
    }).done(e => {
        e.forEach(elem => {
            $("#category_id").append(`<option value="${elem.category_id}">${elem.nameCategory}</option>`)
        })
    })
}
