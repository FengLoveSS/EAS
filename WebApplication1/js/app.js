var app = angular.module('List', ['ui.router', 'ui.bootstrap']);
    
app.config(function ($stateProvider, $urlRouterProvider) {
        
 
    $urlRouterProvider.when("", "/Index/Home");
 
    $stateProvider
       .state("Index", {
           url: "/Index",
           templateUrl: "Page/Page1.html"
       }).state("Index.Home",{
           url: "/Home",
           templateUrl: "Home.html"

       })
        .state("Index.Manage", {
            url:"/Manage",
            templateUrl: "Manage.html"
        })
        .state("Index.Search", {
            url:"/Search",
            templateUrl: "Search.html"
        })
        .state("Index.SC", {
            url:"/SC",
            templateUrl: "SC.html"
        })
        .state("Index.Manage.Student",{
            url:"/Student",
            templateUrl: "Manage/Student_List.html"
        }).state("Index.Manage.Department_List", {
            url: "/Department_List",
            templateUrl:"Manage/Department_List.html"
        }).state("Index.Manage.Department_Edit", {
            url: "/Department_Edit/:DNO",
            templateUrl: "Manage/Department_Edit.html"
        }).state("Index.Manage.Class_List", {
            url: "/Class_List",
            templateUrl: "Manage/Class_List.html"
        }).state("Index.Manage.Class_Edit", {
            url: "/Class_Edit/:CLNO",
            templateUrl: "Manage/Class_Edit.html"
        }).state("Index.Manage.Student_List", {
            url: "/Student_List",
            templateUrl: "Manage/Student_List.html"
        }).state("Index.Manage.Student_Edit", {
            url: "/Student_Edit/:SNO",
            templateUrl: "Manage/Student_Edit.html"
        }).state("Index.Manage.Teacher_List", {
            url: "/Teacher_List",
            templateUrl: "Manage/Teacher_List.html"
        }).state("Index.Manage.Teacher_Edit", {
            url: "/Teacher_Edit/:TNO",
            templateUrl: "Manage/Teacher_Edit.html"
        }).state("Index.Manage.Course_List", {
            url: "/Course_List",
            templateUrl: "Manage/Course_List.html"
        }).state("Index.Manage.Course_Edit", {
            url: "/Course_Edit/:CNO",
            templateUrl: "Manage/Course_Edit.html"
        }).state("Index.SC.Select", {
            url: "/Select",
            templateUrl: "SC/Select.html"
        }).state("Index.SC.Score_List", {
            url: "/Score_List",
            templateUrl: "SC/Score_List.html"
        }).state("Index.SC.Score_Edit", {
            url: "/Score_Edit/:CNO,:SNO",
            templateUrl: "SC/Score_Edit.html"
        }).state("Index.Search.Report", {
            url: "/Report",
            templateUrl: "Search/Report.html"
        }).state("Index.Search.Register", {
            url: "/Register",
            templateUrl: "Search/Register.html"
        });
});
