const idEmp = $("#login-employee-id").val()


$(document).ready(function () {

    let man = {
        manager_id: "Employee0007"
    }

    $('#dataTbl').DataTable({
        ajax: {
            //url: `https://localhost:44302/api/leavingrequest/man/${idEmp}`,
            url: `https://localhost:44302/api/leavingrequest/man/Employee0007`,
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
                                <button type="button" class="btn btn-sm btn-danger" title="Delete" onClick="employeeDelete('Employee/Delete/${data}')" >
                                <i class="fas fa-trash"></i>
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
        $("#request_ids").val(u.request_id);
        $("#employee_id").val(u.employee_id);
        $("#category_id").val(u.category_id);
        $("#startDate").val(u.startDate);
        $("#endDate").val(u.endDate);
        $("#approvalStatusName").val(u.approvalStatusName);
        $("#leavingMessage").val(u.leavingMessage);
        console.log(u)
    })
}

function ApproveReq() {
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/approve",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,
        }),
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}

function RejectReq() {
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/reject",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,
        })
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}

function RevisionReq() {
    var obj = new Object();
    obj.request_id = $("#request_ids").val();
    console.log(obj);
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        url: "https://localhost:44302/api/leavingrequest/man/revisi",
        type: "PATCH",
        contentType: 'application/json',
        data: JSON.stringify(obj)
    }).done((result) => {
        console.log(result)
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: result.message,   
        })
        $('#dataTbl').DataTable().ajax.reload();
    }).fail((error) => {
        console.log(error)
    })
}