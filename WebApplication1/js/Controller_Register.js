app.controller('Register', function ($scope, $http) {
    $scope.page = 1;
    $scope.CNO = "";
    $scope.CName = "";
    $scope.TName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetRegisterMaxPage?CNO=" + $scope.CNO + "&CName=" + $scope.CName + "&TName=" + $scope.TName + "&DNO=" + $scope.Department).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);
        });
    }
    $http.get("../asp.net/Server.asmx/GetAllDepartmentList").success(function (result) {
        result.push({ DNO: "全部", DName: "全部" });//添加全部选项
        $scope.dataset_Department = result;

        $scope.Department = "全部";

        $scope.select(1);

    });
    $scope.select = function (page) {//取数据
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetRegisterList?" + "page=" + page + "&DNO=" + $scope.Department + "&TName=" + $scope.TName + "&CNO=" + $scope.CNO + "&CName=" + $scope.CName).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        getMaxPage();
    };
    
    

});