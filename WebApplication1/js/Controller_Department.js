
//get系的列表
app.controller('Department_List', function ($scope, $http) {
    $scope.page = 1;
    $scope.DNO = "";
    $scope.DName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetDepartmentMaxPage?DNO="+$scope.DNO+"&DName="+$scope.DName).success(function (result) {
            $scope.maxPage = Math.ceil(result / 10);
        });
    }
    $scope.select = function (page) {//取数据
        $scope.dataset = null;
        $scope.run = true;
        $http.get("../asp.net/Server.asmx/GetDepartmentList?" + "page=" + page + "&DName=" + $scope.DName + "&DNO=" + $scope.DNO).success(function (result) {
            $scope.run = false;
            
            $scope.dataset = result;
        });
        getMaxPage();
        
    };
    $scope.delete = function (dno) {//删除按钮
        
        var data = {
            params: {
                DNO: dno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteDepartment", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select(1); }
            else {
                
                alert("发生错误，请检查数据");
            }
        });
    };
    $scope.select(1);
    
});
app.controller('Department_Edit', function ($scope,$http,$stateParams,$state) {
    $scope.isUpdate = false;
    if ($stateParams.DNO == "") {//没有参数时为新建信息
        $scope.Submit = "新建";
        $scope.submit = function () {

            var data = {
                params: {
                    DNO: $scope.DNO,
                    DName: $scope.DName,
                    DIntroduction: $scope.DIntroduction
                }
            };

            $http.get("../asp.net/Server.asmx/NewDepartment", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Department_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
        
        
    }
    else {
        $scope.isUpdate = true;
        $scope.Submit = "修改";
        var data = {
            params: {
                DNO: $stateParams.DNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Department", data).success(function (result) {
            
            $scope.DNO = result[0].DNO;
            $scope.DName = result[0].DName;
            $scope.DIntroduction = result[0].DIntroduction;
        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    DNO: $scope.DNO,
                    DName: $scope.DName,
                    DIntroduction: $scope.DIntroduction
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateDepartment", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Department_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }
    
    $scope.find = function () {//检查是否存在DNO
        var data = {
            params: {
                DNO: $scope.DNO
                
            }
        };

        $http.get("../asp.net/Server.asmx/FindDepartment_DNO", data).success(function (result) {
            if (result == "true")
            {
                $scope.isDNO = true;
            }
            else if(result == "false")
            {
                $scope.isDNO = false;
            }
            
        });
    }


});
