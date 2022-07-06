const idEmp = $("#login-employee-id").val()


$(document).ready(function () {
    GetCategory();
    $('#dataTbl').DataTable({
        ajax: {
            //url: `https://localhost:44302/api/leavingrequest/emp/${idEmp}`,
            url: `https://localhost:44302/api/leavingrequest/emp/Employee0002`,
            "dataType": "json",
            "dataSrc": "",
        },
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
        },
        columns: [
            {
                "data": null,
                "sortable": true,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                },
            },
            {
                "data": "employeeName"
            },
            {
                "data": "categoryName"
            },
            {
                "data": "approvalStatus"
            },
            {
                data: "request_id",
                render: function (data, type, row, meta) {
                    return `<div class="btn-group text-center">
                                <button type="button" class="btn btn-sm btn-warning" title="Delete" onClick="leaveDetail('${data}')" data-toggle="modal" data-target="#updateModal">
                                <i class="fas fa-edit"></i>
                                </button>
                            </div>`
                }
            }
        ]
    });
});

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
        $("#request_id").val(u.request_id);
        $("#employee_id").val(u.employee_id);
        document.getElementById("category_id").value = u.category_id;
        $("#start_date").val(moment(u.startDate).format("MM/DD/yyyy").toString());
        $("#end_date").val(moment(u.endDate).format("MM/DD/yyyy").toString());
        $("#approvalStatusName").val(u.approvalStatusName);
        $("#leavingMessage").val(u.leavingMessage);
        console.log(u)
    })
}

$("#updateReq").submit(function (e) {
    e.preventDefault();
    var obj = new Object();
    obj.request_id = $("#request_id").val();
    obj.employee_id = $("#employee_id").val();
    obj.category_id = parseInt($("#category_id").val()),
    obj.startDate = $("#start_date").val();
    obj.endDate = $("#end_date").val();
    $("#downloadFileBukti").attr("href", "data:application/octet-stream;base64," + obj.fileBukti);
        //obj.fileBukti = $("#fileBukti").val();
    obj.leavingMessage = $("#leavingMessage").val();
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
        //data: obj, //jika terkena 415 unsupported media type (tambahkan headertype Json & JSON.Stringify();)
    }).done((result) => {
        $('#dataTbl').DataTable().ajax.reload();
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Data Updated!',
        })
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
})

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
