$(function () {
    'use strict'

    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    }

    var mode = 'index'
    var intersect = true

    $.ajax({
        type: "GET",
        url: "/DesktopModules/KNTC/API/ServiceKNTC/TiepDanDashBoardGet?year=2022",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //alert(r.tiepDanCharts.length);
            var TiepDan_Thang = [];
            var TiepDan_CoDon = [];
            var TiepDan_KhongDon = [];
            for (var i = 0; i < 12; i++) {
                TiepDan_Thang.push('Tháng ' + (i + 1));
                if (r.tiepDanCharts[i] === undefined) {

                    TiepDan_CoDon.push(0);
                    TiepDan_KhongDon.push(0);
                }
                else {
                    TiepDan_CoDon.push(r.tiepDanCharts[i].CD_Count);
                    TiepDan_KhongDon.push(r.tiepDanCharts[i].KD_Count);
                }
            }
            var $salesChart = $('#tiepdan-chart')
            var salesChart = new Chart($salesChart, {
                type: 'bar',
                data: {
                    labels: TiepDan_Thang,
                    datasets: [
                        {
                            backgroundColor: '#007bff',
                            borderColor: '#007bff',
                            data: TiepDan_CoDon
                        },
                        {
                            backgroundColor: '#ced4da',
                            borderColor: '#ced4da',
                            data: TiepDan_KhongDon
                        }
                    ]
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        mode: mode,
                        intersect: intersect
                    },
                    hover: {
                        mode: mode,
                        intersect: intersect
                    },
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            // display: false,
                            gridLines: {
                                display: true,
                                lineWidth: '4px',
                                color: 'rgba(0, 0, 0, .2)',
                                zeroLineColor: 'transparent'
                            },
                            ticks: $.extend({
                                beginAtZero: true,

                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    if (value >= 1000) {
                                        value /= 1000
                                        value += 'k'
                                    }
                                    return '' + value
                                }
                            }, ticksStyle)
                        }],
                        xAxes: [{
                            display: true,
                            gridLines: {
                                display: false
                            },
                            ticks: ticksStyle
                        }]
                    }
                }
            })
            var LoaiTiepDan_Content = $('.LoaiTiepDan_Content');
            for (var i = 0; i < r.TD_LoaiDonThu.length; i++) {

                var SoLuongsoVoiNamTruoc = r.TD_LoaiDonThu[i].Count - r.TD_LoaiDonThu[i].Count_oldYear;
                var str = "";
                if (SoLuongsoVoiNamTruoc < 0) {
                    str = "<small class='text-warning mr-1'> <i class='icofont-caret-down mr-r5' ></i ><b>" + (r.TD_LoaiDonThu[i].Count_oldYear - r.TD_LoaiDonThu[i].Count) + "</b></small> So cùng kỳ năm trước"
                }
                else if (SoLuongsoVoiNamTruoc > 0) {
                    str = "<small class='text-success mr-1'> <i class='icofont-caret-up  mr-r5'></i ><b>" + (r.TD_LoaiDonThu[i].Count - r.TD_LoaiDonThu[i].Count_oldYear) + "</b></small> So cùng kỳ năm trước"
                }
                else {
                    str = "Bằng so cùng kỳ năm trước"
                }

                var str_loaitiepdan = "";
                str_loaitiepdan += "<div class='col-md-3 col-sm-6 col-12'>";
                str_loaitiepdan += "<div class='info-box bg-default'>";
                str_loaitiepdan += "<div class='info-box-content'>";
                str_loaitiepdan += "<span class='info-box-text mr-t10 ' style='white-space: normal;'><h6>" + r.TD_LoaiDonThu[i].LOAIDONTHU_TEN.toUpperCase() +"</h6></span>";
                str_loaitiepdan += "<span class='info-box-number'><h3>" + r.TD_LoaiDonThu[i].Count +"</h3></span>";
               // str_loaitiepdan += "<div class='progress'> <div class='progress-bar' style='width: 70%'></div></div>";
                str_loaitiepdan += "<span class=' mr-b10 mr-t10 ' style='white-space: normal;' >" + str+" </span>";//progress-description
                str_loaitiepdan += "</div>";
                str_loaitiepdan += "<span class='info-box-icon img-circle' style='border-radius: 50%;min-width: 60px;width: 60px;height: 60px;color: #fff;background-color:" + r.TD_LoaiDonThu[i].LOAIDONTHU_COLOR.replace("0xFF","#") +";text-align: right;margin-top: 20px;'><i class='" + r.TD_LoaiDonThu[i].LOAIDONTHU_ICONNAME+"'></i></span>";
                str_loaitiepdan += "</div></div>";
                LoaiTiepDan_Content.append(str_loaitiepdan);
                //<div class="col-md-3 col-sm-6 col-12">
                //    <div class="info-box bg-default">
                //        <div class="info-box-content">
                //            <span class="info-box-text">KHIẾU NẠI</span>
                //            <span class="info-box-number">
                //                <h4>41</h4>
                //            </span>
                //            <div class="progress">
                //                <div class="progress-bar" style="width: 70%"></div>
                //            </div>
                //            <span class="progress-description">70% Increase in 30 Days
                //            </span>
                //        </div>
                //        <span class="info-box-icon"><i class="far fa-bookmark"></i></span>
                //    </div>
                //</div>
            }
            
        },
        error: function (r) {
            alert(r.responseText);
        },
        failure: function (r) {
            alert(r.responseText);
        }
    });

    $.ajax({
        type: "GET",
        url: "/DesktopModules/KNTC/API/ServiceKNTC/TiepDan_DoiTuong_Loai?year=2022",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //alert(r.tiepDanCharts.length);
            var TiepDan_DoiTuong_Loai_Total = [];
            var TiepDan_DoiTuong_Loai_Name = [];
            TiepDan_DoiTuong_Loai_Total.push(r[1].DOITUONGLOAI_COUNT);
            TiepDan_DoiTuong_Loai_Total.push(r[2].DOITUONGLOAI_COUNT);
            TiepDan_DoiTuong_Loai_Total.push(r[3].DOITUONGLOAI_COUNT);
            TiepDan_DoiTuong_Loai_Name.push(r[1].DOITUONGLOAI_TEN);
            TiepDan_DoiTuong_Loai_Name.push(r[2].DOITUONGLOAI_TEN);
            TiepDan_DoiTuong_Loai_Name.push(r[3].DOITUONGLOAI_TEN);
            
            $('.total-td-cn').append(r[1].DOITUONGLOAI_COUNT);
            $('.total-td-cqtc').append(r[3].DOITUONGLOAI_COUNT);
            $('.total-td-ddn').append(r[2].DOITUONGLOAI_COUNT);
            if (r[0].DOITUONGLOAI_COUNT >0) {
            $('.total-td-cn-per').append(((r[1].DOITUONGLOAI_COUNT / r[0].DOITUONGLOAI_COUNT) * 100).toFixed(1) +"%") ;
            $('.total-td-cqtc-per').append(((r[3].DOITUONGLOAI_COUNT / r[0].DOITUONGLOAI_COUNT) * 100).toFixed(1) + "%");
            $('.total-td-ddn-per').append(((r[2].DOITUONGLOAI_COUNT / r[0].DOITUONGLOAI_COUNT) * 100).toFixed(1) + "%");
             }
            //---------------------------
            //- END MONTHLY SALES CHART -
            //---------------------------

            //-------------
            //- PIE CHART -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.

            var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
            var pieData = {
                labels: TiepDan_DoiTuong_Loai_Name,
                datasets: [
                    {
                        data: TiepDan_DoiTuong_Loai_Total,
                        backgroundColor: ['#1070ca', '#f7d154', '#ec4c47'],
                    }
                ]
            }
            var pieOptions = {
                legend: {
                    display: false
                }
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            var pieChart = new Chart(pieChartCanvas, {
                type: 'doughnut',
                data: pieData,
                options: pieOptions
            })
        },
        error: function (r) {
            alert(r.responseText);
        },
        failure: function (r) {
            alert(r.responseText);
        }
    });


    ///
    /// ĐƠN THƯ
    ///
    var ChartDonThu_content = $('.ChartDonThu_content .ChartDonThu_title');
    $.ajax({
        type: "GET",
        url: "/DesktopModules/KNTC/API/ServiceKNTC/DonThuDashBoardGet?year=2022",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            //alert(r.tiepDanCharts.length);
            var DonThu_DataSet = [];
            
            var DonThu_TenLoai = [];
            var DonThu_Corlor = ['', ''];

            var LoaiDonThu_Color = ['#007bff', '#dc3545', '#ffc107', '#28a745', '#6c757d', '#17a2b8', '#f8f9fa'];
            var LoaiDonThu_ColorName = ['primary', 'danger', 'warning ', 'success', 'secondary ', 'info', 'default'];
            for (var i = 0; i < r.LoaiDonThu.length; i++) {
                /*<span><i class="fas fa-square text-gray"></i>Không đơn</span>*/
                var DonThu_Thang = [];
                DonThu_TenLoai.push(r.LoaiDonThu[i].LOAIDONTHU_TEN);
                DonThu_Thang.push(r.LoaiDonThu[i].T1);
                DonThu_Thang.push(r.LoaiDonThu[i].T2);
                DonThu_Thang.push(r.LoaiDonThu[i].T3);
                DonThu_Thang.push(r.LoaiDonThu[i].T4);
                DonThu_Thang.push(r.LoaiDonThu[i].T5);
                DonThu_Thang.push(r.LoaiDonThu[i].T6);
                DonThu_Thang.push(r.LoaiDonThu[i].T7);
                DonThu_Thang.push(r.LoaiDonThu[i].T8);
                DonThu_Thang.push(r.LoaiDonThu[i].T9);
                DonThu_Thang.push(r.LoaiDonThu[i].T10);
                DonThu_Thang.push(r.LoaiDonThu[i].T11);
                DonThu_Thang.push(r.LoaiDonThu[i].T12);

                var DonThu_Data = {};
                DonThu_Data['backgroundColor'] = LoaiDonThu_Color[i];
                DonThu_Data['borderColor'] = LoaiDonThu_Color[i];
                DonThu_Data['data'] = DonThu_Thang;
                DonThu_DataSet.push(DonThu_Data);
                ChartDonThu_content.append("<span class='mr-10'><i class='fas fa-square text-" + LoaiDonThu_ColorName[i] + "'></i> " + r.LoaiDonThu[i].LOAIDONTHU_TEN+"</span>");
                //DonThu_DataSet[i].push({ 'backgroundColor':'#007bff' });

                //DonThu_DataSet[i]['backgroundColor'] = '#007bff'
                //DonThu_DataSet[i]['data'] = DonThu_Thang;
            }

                var $salesChart = $('#donthu-chart')
                var salesChart = new Chart($salesChart, {
                    type: 'bar',
                    data: {
                        labels: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                        datasets: DonThu_DataSet
                        //[
                        //    {
                        //        backgroundColor: '#007bff',
                        //        borderColor: '#007bff',
                        //        data: TiepDan_CoDon
                        //    },
                        //    {
                        //        backgroundColor: '#ced4da',
                        //        borderColor: '#ced4da',
                        //        data: TiepDan_KhongDon
                        //    }
                        //]
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            mode: mode,
                            intersect: intersect
                        },
                        hover: {
                            mode: mode,
                            intersect: intersect
                        },
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                // display: false,
                                gridLines: {
                                    display: true,
                                    lineWidth: '4px',
                                    color: 'rgba(0, 0, 0, .2)',
                                    zeroLineColor: 'transparent'
                                },
                                ticks: $.extend({
                                    beginAtZero: true,

                                    // Include a dollar sign in the ticks
                                    callback: function (value, index, values) {
                                        if (value >= 1000) {
                                            value /= 1000
                                            value += 'k'
                                        }
                                        return '' + value
                                    }
                                }, ticksStyle)
                            }],
                            xAxes: [{
                                display: true,
                                gridLines: {
                                    display: false
                                },
                                ticks: ticksStyle
                            }]
                        }
                    }
                })

            var TiepDan_DoiTuong_Loai_Total = [];
            var TiepDan_DoiTuong_Loai_Name = [];
            TiepDan_DoiTuong_Loai_Total.push(r.CaNhan);
            TiepDan_DoiTuong_Loai_Total.push(r.CoQuan);
            TiepDan_DoiTuong_Loai_Total.push(r.ToChuc);
            TiepDan_DoiTuong_Loai_Name.push("Cá nhân");
            TiepDan_DoiTuong_Loai_Name.push("Cơ quan tổ chức");
            TiepDan_DoiTuong_Loai_Name.push("Đoàn đông người");
            var Total_DonThu_Loai_DoiTuong = r.CaNhan + r.CoQuan + r.ToChuc;

            $('.total-dt-cn').append(r.CaNhan);
            $('.total-dt-cqtc').append(r.CoQuan);
            $('.total-dt-ddn').append(r.ToChuc);
            if (Total_DonThu_Loai_DoiTuong >0) {
            $('.total-dt-cn-per').append(((r.CaNhan / Total_DonThu_Loai_DoiTuong) * 100).toFixed(1) + "%");
            $('.total-dt-cqtc-per').append(((r.CoQuan / Total_DonThu_Loai_DoiTuong) * 100).toFixed(1) + "%");
            $('.total-dt-ddn-per').append(((r.ToChuc / Total_DonThu_Loai_DoiTuong) * 100).toFixed(1) + "%");
            }
            //---------------------------
            //- END MONTHLY SALES CHART -
            //---------------------------

            //-------------
            //- PIE CHART -
            //-------------
            // Get context with jQuery - using jQuery's .get() method.
            var pieChartCanvas = $('#pieChartDonThu').get(0).getContext('2d')
            var pieData = {
                labels: TiepDan_DoiTuong_Loai_Name,
                datasets: [
                    {
                        data: TiepDan_DoiTuong_Loai_Total,
                        backgroundColor: ['#1070ca', '#f7d154', '#ec4c47'],
                    }
                ]
            }
            var pieOptions = {
                legend: {
                    display: false
                }
            }
            //Create pie or douhnut chart
            // You can switch between pie and douhnut using the method below.
            var pieChart = new Chart(pieChartCanvas, {
                type: 'doughnut',
                data: pieData,
                options: pieOptions
            })


            var LoaiDonThu_Content = $('.LoaiDonThu_Content');
            for (var i = 0; i < r.HuongXuLy.length; i++) {

                var SoLuongsoVoiNamTruoc = r.HuongXuLy[i].HUONGXYLY_Count - r.HuongXuLy[i].HUONGXYLY_Count_OldYear;
                var str = "";
                if (SoLuongsoVoiNamTruoc < 0) {
                    str = "<small class='text-warning mr-1'> <i class='icofont-caret-down mr-r5' ></i ><b>" + (r.HuongXuLy[i].HUONGXYLY_Count_OldYear - r.HuongXuLy[i].HUONGXYLY_Count) + "</b></small> So cùng kỳ năm trước"
                }
                else if (SoLuongsoVoiNamTruoc > 0) {
                    str = "<small class='text-success mr-1'> <i class='icofont-caret-up  mr-r5'></i ><b>" + (r.HuongXuLy[i].HUONGXYLY_Count - r.HuongXuLy[i].HUONGXYLY_Count_OldYear) + "</b></small> So cùng kỳ năm trước"
                }
                else {
                    str  = "Bằng so cùng kỳ năm trước"
                }

                var str_loaidonthu = "";
                str_loaidonthu += "<div class='col-xxl-2 col-md-3 col-sm-6 col-12'>";
                str_loaidonthu += "<div class='info-box bg-default'>";
                str_loaidonthu += "<div class='info-box-content'>";
                str_loaidonthu += "<span class='info-box-text mr-t10' style='white-space: normal;'><h6>" + r.HuongXuLy[i].HUONGXYLY_TEN.toUpperCase() +"</h6></span>";
                str_loaidonthu += "<span class='info-box-number'><h3>" + r.HuongXuLy[i].HUONGXYLY_Count +"</h3></span>";
                //str_loaidonthu += "<div class='progress'> <div class='progress-bar' style='width: 70%'></div></div>";
                str_loaidonthu += "<span class=' mr-b10 mr-t10 ' style='white-space: normal;' >" + str+" </span>";
                str_loaidonthu += "</div>";
                str_loaidonthu += "<span class='info-box-icon img-circle' style='border-radius: 50%;min-width: 60px;width: 60px;height: 60px;color: #fff;background-color:" + r.HuongXuLy[i].HUONGXULY_COLOR.replace("0xFF","#") + ";text-align: right;margin-top: 20px;'><i class='" + r.HuongXuLy[i].HUONGXULY_ICONNAME +"'></i></span>";
                str_loaidonthu += "</div></div>";
                LoaiDonThu_Content.append(str_loaidonthu);
                //<div class="col-md-3 col-sm-6 col-12">
                //    <div class="info-box bg-default">
                //        <div class="info-box-content">
                //            <span class="info-box-text">KHIẾU NẠI</span>
                //            <span class="info-box-number">
                //                <h4>41</h4>
                //            </span>
                //            <div class="progress">
                //                <div class="progress-bar" style="width: 70%"></div>
                //            </div>
                //            <span class="progress-description">70% Increase in 30 Days
                //            </span>
                //        </div>
                //        <span class="info-box-icon"><i class="far fa-bookmark"></i></span>
                //    </div>
                //</div>
            }
            //<div class="col-md-3 col-sm-6 col-12">
            //    <div class="info-box bg-default">
            //        <div class="info-box-content">
            //            <span class="info-box-text">KHIẾU NẠI</span>
            //            <span class="info-box-number">
            //                <h4>41</h4>
            //            </span>
            //            <div class="progress">
            //                <div class="progress-bar" style="width: 70%"></div>
            //            </div>
            //            <span class="progress-description">70% Increase in 30 Days
            //            </span>
            //        </div>
            //        <span class="info-box-icon"><i class="far fa-bookmark"></i></span>
            //    </div>
            //</div>
        },
        error: function (r) {
            alert(r.responseText);
        },
        failure: function (r) {
            alert(r.responseText);
        }
    });

})