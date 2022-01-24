var bar = new ApexCharts(document.querySelector("#bar"), barOptions);
var area = new ApexCharts(document.querySelector("#area"), areaOptions);
var url = 'http://cntttest.vanlanguni.edu.vn:18080/CP24Team08/Admin/Dashboard/GetData';
var urlLocal = 'https://localhost:44328/Admin/Dashboard/GetData';
$.getJSON(url, function (res) {
	bar.updateOptions({
		series: [{
			data: res.priceExaminationInfo
		},
		{
			data: res.pricePrescriptionDetail
		},
		{
			data: res.priceTestSubclinical
		}],
		xaxis: {
			categories: res.datetime
		}
	});
})

$.getJSON(url, function (res) {
	area.updateOptions({
		series: [{
			data: res.numUser
		}],
		xaxis: {
			categories: res.datetime
		}
	});
})
bar.render();
area.render();

