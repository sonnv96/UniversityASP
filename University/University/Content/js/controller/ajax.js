

$(document).ready(function () {
    _getAll();
});

function _getAll() {
    $.ajax({
        url: "/SinhVien/Get",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.monhoc + '</td>';

                html += '<td><a href="#" onclick="return _getById(' + item.monhoc + ')">Edit</a> | <a href="#" onclick="return _delete(' + item.monhoc + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('#list tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

  
    return false;
}
function _getById(id) {
    $.ajax({
        url: '/SinhVien/Get2/' + id,
        // data: JSON.stringify(dto),
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                
                html += '<p>' + item.monhoc + '</p>';
                html += '<p>' + item.tengiangvien+ '</p>';
                html += '<p>' + item.soluong + '</p>';

             
            });
            $('#show').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
function _add() {
    var obj = {
        StudentId: $('#StudentId').val(),
        Name: $('#Name').val(),
        Status: $('#Status').val(),
    }
    $.ajax({
        url: '/Home/Create',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            _getAll();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function _edit() {
    var obj = {
        StudentId: $('#StudentId').val(),
        Name: $('#Name').val(),
        Status: $('#Status').val(),
    }
    $.ajax({
        url: '/Home/Update',
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",

        success: function (result) {
            _getAll();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function _delete(id) {
    var cf = confirm('Are you sure want to permanently delete this row?');
    if (cf) {
        $.ajax({
            url: '/Home/Delete/' + id,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (result) {
                _getAll();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}