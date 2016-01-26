app.controller('Teacher_List', function ($scope, $http) {
    $scope.page = 1;
    $scope.TNO = "";
    $scope.TName = "";
    var getMaxPage = function () {//获得最大的页码数
        $http.get("../asp.net/Server.asmx/GetTeacherMaxPage?TNO=" + $scope.TNO + "&TName=" + $scope.TName + "&DNO=" + $scope.Department).success(function (result) {
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
        $http.get("../asp.net/Server.asmx/GetTeacherList?" + "page=" + page + "&DNO=" + $scope.Department + "&TName=" + $scope.TName + "&TNO=" + $scope.TNO).success(function (result) {
            $scope.dataset = result;
            $scope.run = false;
        });
        getMaxPage();
    };
    $scope.delete = function (tno) {//删除按钮

        var data = {
            params: {
                TNO: tno

            }
        };
        $http.get("../asp.net/Server.asmx/DeleteTeacher", data).success(function (result) {
            if (result == "成功") { alert("成功"); $scope.select($scope.page); }
            else {
                alert("发生错误，请检查数据");
            }
        });
    };
    

});
app.controller('Teacher_Edit', function ($scope, $http, $stateParams, $state) {

    $scope.isUpdate = false;//初始化状态
    $scope.TSex = "男";
    $scope.TTitle = "普通教师";
    $scope.TBrithday = new Date();
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


    if ($stateParams.TNO == "") {

        $scope.Submit = "新建";
        $scope.submit = function () {//新建新班级

            var data = {
                params: {
                    TNO: $scope.TNO,
                    TName: $scope.TName,
                    TSex: $scope.TSex,
                    TBrithday: $scope.TBrithday,
                    DNO: $scope.Department,
                    TTitle: $scope.TTitle,
                    TIntroduction: $scope.TIntroduction
                }
            };
            
            $http.get("../asp.net/Server.asmx/NewTeacher", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Teacher_List', $stateParams); }
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
                TNO: $stateParams.TNO
            }
        };

        $http.get("../asp.net/Server.asmx/GetSingle_Teacher", data).success(function (result) {//填充数据

            $scope.TNO = result[0].TNO;
            $scope.TName = result[0].TName;
            $scope.TSex = result[0].TSex;
            $scope.TBrithday = result[0].TBrithday;
            $scope.Department = result[0].DNO;
            $scope.TTitle = result[0].TTitle;
            $scope.TIntroduction = result[0].TIntroduction;


        });
        $scope.submit = function () {//有参数时就变为更新了

            var data = {
                params: {
                    TNO: $scope.TNO,
                    TName: $scope.TName,
                    TSex: $scope.TSex,
                    TBrithday: $scope.TBrithday,
                    DNO: $scope.Department,
                    TTitle: $scope.TTitle,
                    TIntroduction: $scope.TIntroduction
                }
            };

            $http.get("../asp.net/Server.asmx/UpdateTeacher", data).success(function (result) {
                if (result == "成功") { alert("成功"); $state.go('Index.Manage.Teacher_List', $stateParams); }
                else {

                    alert("发生错误，请检查数据");
                }
            });


        };
    }

    $scope.find = function () {//查找是否存在该班级号

        var data = {
            params: {
                TNO: $scope.TNO

            }
        };

        $http.get("../asp.net/Server.asmx/FindTeacher_TNO", data).success(function (result) {
            if (result == "true") {
                $scope.isTNO = true;
            }
            else if (result == "false") {
                $scope.isTNO = false;
            }


        });
    }


});