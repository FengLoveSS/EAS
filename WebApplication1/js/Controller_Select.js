app.controller('Select', function ($scope, $http) {
    $scope.page_Course = 1;
    $scope.page_Student = 1;
    var getMaxPage_Student = function () {//获得学生最大的页码数
        $http.get("../asp.net/Server.asmx/GetStudentMaxPage?DNO=" + $scope.Department + "&SYear=" + $scope.SYear).success(function (result) {
            $scope.maxPage_Student = Math.ceil(result / 10);
            

        });
    }
    var getMaxPage_Course = function () {//获得课程最大的页码数
        $http.get("../asp.net/Server.asmx/GetCourseMaxPage?DNO=" + $scope.Department).success(function (result) {
            $scope.maxPage_Course = Math.ceil(result / 10);


        });
    }
    $scope.select_Student = function (page) {//取学生数据

        $http.get("../asp.net/Server.asmx/GetStudentList?" + "page=" + page + "&DNO=" + $scope.Department + "&SYear=" + $scope.SYear).success(function (result) {
            $scope.dataset_Student = result;
            getMaxPage_Student();
            
        });

    };
    $scope.select_Course = function (page) {//取课程数据

        $http.get("../asp.net/Server.asmx/GetCourseList?" + "page=" + page + "&DNO=" + $scope.Department).success(function (result) {
            $scope.dataset_Course = result;
            getMaxPage_Course();
        });
        
    };
    $http.get("../asp.net/Server.asmx/GetAllDepartmentList").success(function (result) {
        
        $scope.dataset_Department = result;
        
        
            $scope.Department = result[0].DNO;
        
        $scope.select_Student(1);
        $scope.select_Course(1);
    });
    $http.get("../asp.net/Server.asmx/GetAllSYearList").success(function (result) {//填充年份
        result.push({ SYear: "全部" });//添加全部选项
        $scope.dataset_SYear = result;

        $scope.SYear = "全部";

        $scope.select_Student(1);

    });
    var students = [];//用于保存选课的学生
    var courses = [];//用于保存选课的课程
    $scope.CheckIn_Student = function ($event, sno) {
        if ($event.target.checked) {//判断是否被选中
            students.push(sno);//推进
        }
        else {
            students.splice(students.indexOf(sno), 1);//删除
        }
        

    };
    $scope.CheckIn_Course = function ($event, cno) {
        if ($event.target.checked) {//判断是否被选中
            courses.push(cno);//推进
        }
        else {
            courses.splice(courses.indexOf(cno), 1);//删除
        }
        

    };
    $scope.submit = function () {
        var data = {
            params: {
                students:students,
                courses: courses
                }
        };
        
        $http.get("../asp.net/Server.asmx/SignUp", data).success(function (result) {
            if (result != "成功") alert("错误:" + result);
            else alert(result);
            
            if (result == "成功") {
                $state.go('Index.SC.Score_List', $stateParams);
            }


        });
    };
    
    
    

});