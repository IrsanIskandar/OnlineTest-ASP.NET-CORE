﻿"use strict"

function isNumber(evt) {
    var iKeyCode = (evt.which) ? evt.which : evt.keyCode
    if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
        return false;

    return true;
};

function RegisterUser() {
    const fullName = $('#inputNama').val();
    const hpNumber = $('#inputNoTlp').val();
    const division = $('#ddlDivisi option:selected').index();
    const loginEnabled = 'btn btn-lg btn-primary btn-block';
    const loginDisabled = 'btn btn-lg btn-block disabled';

    //$('#btnRegister').removeClass(loginEnabled).addClass(loginDisabled);

    if (fullName !== '' && hpNumber.length !== 0 && division !== 0) {
        //alert("Oke this working!");
        $('#btnRegister').removeClass(loginEnabled).addClass(loginDisabled);
        $('#btnRegister').css('cursor', 'wait');
        //$('#btnRegister').prop("disabled", true);
        $('#btnRegister').empty().val('Please Wait...');
    }
};

$(document).ready(function () {
    $('#btnRegister').click(function () {
        RegisterUser();
    });

    $('#inputNama').keypress(function (e) {
        if (e.which === 13) {
            RegisterUser();
        }
    });

    $('#inputNoTlp').keypress(function (e) {
        if (e.which === 13) {
            RegisterUser();
        }
    });

    //$('#ddlDivisi').change(function (e) {
    //    RegisterUser();
    //});
});