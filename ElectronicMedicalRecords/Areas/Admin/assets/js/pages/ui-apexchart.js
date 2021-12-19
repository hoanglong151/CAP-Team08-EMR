var bar = new ApexCharts(document.querySelector("#bar"), barOptions);
var url = 'https://localhost:44328/Admin/Dashboard/GetData';
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

bar.render();

