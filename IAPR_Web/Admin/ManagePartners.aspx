<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManagePartners.aspx.cs" Inherits="IAPR_Web.Admin.ManagePartners" %>

<%@ Register Src="../UserControls/Admin/AddPartner.ascx" TagName="AddPartner" TagPrefix="uc2" %>



<%@ Register Src="../UserControls/Admin/AddPartnerUser.ascx" TagName="AddPartnerUser" TagPrefix="uc1" %>



<%@ Register Src="../UserControls/Admin/EditPartnerUser.ascx" TagName="EditPartnerUser" TagPrefix="uc3" %>



<%@ Register Src="../UserControls/Admin/EditPartner.ascx" TagName="EditPartner" TagPrefix="uc4" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Manage partners</h1>
        <div>
            <div class="form-row align-items-center">
                <div class="form-group col-md-6">
                    <label class="txtFieldLabel">Select process</label>
                    <asp:DropDownList CssClass="form-control" ID="ddlFunction" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="1">Add new partner</asp:ListItem>
                        <asp:ListItem Value="4">Edit partner</asp:ListItem>
                        <asp:ListItem Value="2">Add new user</asp:ListItem>
                        <asp:ListItem Value="3">Edit user</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ddlFunction" Display="Dynamic" SetFocusOnError="true" ValidationGroup="vgManagePartner" CssClass="txtnamevalidation erroMessage" ID="RequiredFieldValidator5" ErrorMessage="This field is required" runat="server"></asp:RequiredFieldValidator>

                </div>
            </div>
        </div>

        <asp:Button CssClass="btn btn-primary" ID="btnContinue" ValidationGroup="vgManagePartner" runat="server" Text="Continue" OnClick="btnContinue_Click" />
    </div>
    <asp:Panel ID="pnlAddPartner" runat="server" Visible="false">
        <uc2:AddPartner ID="AddPartner1" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlAddPartnerUser" runat="server" Visible="false">
        <uc1:AddPartnerUser ID="AddPartnerUser1" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlEditPartnerUser" runat="server" Visible="false">
        <uc3:EditPartnerUser ID="EditPartnerUser1" runat="server" />

    </asp:Panel>
    <asp:Panel ID="pnlEditPartner" runat="server" Visible="false">
        <uc4:EditPartner ID="EditPartner" runat="server" />
    </asp:Panel>
    <script>
        $(document).ready(function () {
            options();
            visible();
            showinAddTransporter();
            showoutAddTransporter();
            //showinAddTransporterDriver();
            //showoutAddTransporterDriver();

        });

        function options() {
            $('.downArrow').click(function () {
                $('.downArrow').toggleClass('upArrow');
                $('.optionsContainer').toggle();
            });
            $('.existClose,.thankPopClose').click(function (e) {
                e.preventDefault();
                location.reload();
            });
            $('.tncLink').attr("href", "/BCP/Pages/WW-Terms.aspx");

            $('.specifications').click(function () {

                $('.specifications').toggleClass('closeSpec');
                $('.divSpecs').toggle();
            });

        }
        function visible() {
            if ($(".searchFilter").is(":visible")) {
                $('section.wwHeaderImg').show();
            } else {
                $('section.wwHeaderImg').hide();
            }
        }
        function showinAddTransporter() {
            $('.addTranspoter').click(function (e) {
                e.preventDefault();
                $('#divAddTranspoter').addClass('in');
                $('.modal').css('display', 'block');
                $('body').append('<div class="modal-backdrop"></div>');

            });

        }
        function showoutAddTransporter() {
            $('.modalCloseIcon,.formClose').click(function (e) {
                e.preventDefault();
                $('#divAddTranspoter').removeClass('in');
                $('.modal').css('display', 'none');
                $('<div class="modal-backdrop"></div>').remove();

                $('.modal-backdrop,.exist,.thankPop').remove();

            });
        }

        //function showinAddTransporterDriver() {
        //    $('.addTranspoterDriver').click(function (e) {
        //        e.preventDefault();
        //        $('#divAddTranspoterDriver').addClass('in');
        //        $('.modal').css('display', 'block');
        //        $('body').append('<div class="modal-backdrop"></div>');

        //    });

        //}
        //function showoutAddTransporterDriver() {
        //    $('.modalCloseIcon,.formClose').click(function (e) {
        //        e.preventDefault();
        //        $('#divAddTranspoterDriver').removeClass('in');
        //        $('.modal').css('display', 'none');
        //        $('<div class="modal-backdrop"></div>').remove();

        //        $('.modal-backdrop,.exist,.thankPop').remove();

        //    });
        //}



        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(options);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(visible);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(showinAddTransporter);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(showoutAddTransporter);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(showinAddTransporterDriver);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(showoutAddTransporterDriver);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(identityChanger);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(validateIDFields);

        function identityChanger() {
            $('.woolworthsRadioButtons input').on('click', function () {
                var valOFRad = $(this).attr("value");
                if (valOFRad == 1) {
                    $('*[data-target="SAId"]').show();
                    $('*[data-target="PassportNumber"]').hide();
                    $('*[data-target="RefugeeNumber"]').hide();

                }
                else if (valOFRad == 2) {
                    $('*[data-target="SAId"]').hide();
                    $('*[data-target="PassportNumber"]').show();
                    $('*[data-target="RefugeeNumber"]').hide();
                }
                else {
                    $('*[data-target="SAId"]').hide();
                    $('*[data-target="PassportNumber"]').hide();
                    $('*[data-target="RefugeeNumber"]').show();
                }
            });
        }

        function validateIDFields() {

            $('.btnViewdetails').on('click', function () {
                //console.log("Checking")
                return validateIDTXTBox();
            });

            function validateIDTXTBox() {
                var idReg = /(((\d{2}((0[13578]|1[02])(0[1-9]|[12]\d|3[01])|(0[13456789]|1[012])(0[1-9]|[12]\d|30)|02(0[1-9]|1\d|2[0-8])))|([02468][048]|[13579][26])0229))(( |-)(\d{4})( |-)(\d{3})|(\d{7}))/gm;
                if ($('.woolworthsRadioButtons input:checked').val() == "1") {
                    //console.log("Validate ID")
                    var idTxt = $('*[data-target="SAId"] input').val();
                    if (idTxt != "") {
                        //Verify if ID
                        if (idReg.test(idTxt)) {
                            $('*[data-target="SAId"] span').hide();
                            return true;
                        } else {
                            $('*[data-target="SAId"] span').hide();
                            $($('*[data-target="SAId"] span')[1]).show()
                            return false;
                        }
                    }
                    else {
                        $('*[data-target="SAId"] span').hide();
                        $($('*[data-target="SAId"] span')[0]).show()
                        return false;
                    }
                } else if ($('.woolworthsRadioButtons input:checked').val() == "2") {
                    // console.log("Passport Validate");
                    //Validate Passport
                    if ($('*[data-target="PassportNumber"] input').val() != "") {
                        $('*[data-target="PassportNumber"] span').hide();
                        return true;
                    } else {
                        $('*[data-target="PassportNumber"] span').show();
                        return false;
                    }
                } else {
                    //Validate Refugee
                    //console.log("Validate Refugee")
                    if ($('*[data-target="RefugeeNumber"] input').val() != "") {
                        $('*[data-target="RefugeeNumber"] span').hide();
                        return true;
                    } else {
                        $('*[data-target="RefugeeNumber"] span').show();
                        return false;
                    }
                }

            }

            //check on blur
            $('*[data-target="PassportNumber"] input').on('blur', function () {
                validateIDTXTBox()
            });
            $('*[data-target="SAId"] input').on('blur', function () {
                validateIDTXTBox()
            });
            $('*[data-target="RefugeeNumber"] input').on('blur', function () {
                validateIDTXTBox()
            });


        }


    </script>
</asp:Content>
