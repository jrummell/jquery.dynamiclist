/// <binding />
var gulp = require("gulp");
var uglify = require("gulp-uglifyjs");
var eol = require("gulp-eol");

gulp.task("default", function () {
    const dist = "dist";
    gulp.src("src/jquery.dynamiclist.js")
        .pipe(uglify("jquery.dynamiclist.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest(dist));

    gulp.src("src/jquery.dynamiclist.templates.bootstrap.js")
        .pipe(uglify("jquery.dynamiclist.templates.bootstrap.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest(dist));

    gulp.src("src/jquery.dynamiclist.templates.kendo.js")
        .pipe(uglify("jquery.dynamiclist.templates.kendo.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest(dist));

    gulp.src("src/jquery.validate.unobtrusive.dynamic.js")
        .pipe(uglify("jquery.validate.unobtrusive.dynamic.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest(dist));

    const exampleDest = "./example/jquery.dynamiclist.web/Scripts";

    gulp.src("src/*.js")
        .pipe(gulp.dest(exampleDest));
});