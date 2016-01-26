app.controller('Class_List', function ($scope, $http) {
    $scope.page = 1;
    $scope.CLNO = "";
    $scope.CLName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetClassMaxPage?CLNO=" + $scope.CLNO + "&CLName=" + $scope.CLName + "&DNO=" + $scope.Department).success(function (result) {
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
        $http.get("../asp.net/Server.asmx/GetClassList?" + "page=" + page + "&DNO=" + $scope.Department + "&CLName=" + $scope.CLName + "&CLNO=" + $scope.CLNO).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        
    };
    $scope.delete = function (clno) {//删除按钮

        var data = {
            params: {
                CLNO: clno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteClass", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select(); }
            else {
                alert("发生错误，请检查数据");
            }
        });
    };
    

});
app.controller('Class_Edit', function ($scope, $http, $stateParams, $state) {

    $scope.isUpdate = false;
    $http.get("../asp.net/Server.asmx/GetAllDepartmentList").success(function (result) {

        $scope.dataset = result;
        if (result.length != 0) {
            $scope.issDNO = true;//判断是否存在系可以选，此时为存在
            if ($scope.Submit == "新建")
            $scope.Department = result[0].DNO;
        } else {
            $scope.issDNO = false;
        }
        

    });
    
    
    if ($stateParams.CLNO == "") {
        
        $scope.Submit = "新建";
        $scope.submit = function () {//新建新班级

            var data = {
                params: {
                    CLNO: $scope.CLNO,
                    CLName: $scope.CLName,
                    DNO: $scope.Department
                }
            };

            $http.get("../asp.net/Server.asmx/NewClass", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Class_List', $stateParams); }
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
                CLNO: $stateParams.CLNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Class", data).success(function (result) {//填充数据
            
            $scope.CLNO = result[0].CLNO;
            $scope.CLName = result[0].CLName;
            $scope.Department = result[0].DNO;
            
            
        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    CLNO: $scope.CLNO,
                    CLName: $scope.CLName,
                    DNO: $scope.Department
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateClass", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Class_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }

    $scope.find = function () {//查找是否存在该班级号

        var data = {
            params: {
                CLNO: $scope.CLNO

            }
        };

        $http.get("../asp.net/Server.asmx/FindClass_CLNO", data).success(function (result) {
            if (result == "true") {
                $scope.isCLNO = true;
            }
            else if (result == "false") {
                $scope.isCLNO = false;
            }


        });
    }


});