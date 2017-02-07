/// <binding AfterBuild='uglify' />
var gulp = require("gulp");
var uglify = require("gulp-uglifyjs");
var eol = require("gulp-eol");

gulp.task("uglify", function ()
{
    gulp.src("scripts/package/jquery.dynamiclist.js")
        .pipe(uglify("jquery.dynamiclist.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest("scripts/package"));

    gulp.src("scripts/package/jquery.dynamiclist.templates.bootstrap.js")
        .pipe(uglify("jquery.dynamiclist.templates.bootstrap.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest("scripts/package"));

    gulp.src("scripts/package/jquery.dynamiclist.templates.kendo.js")
        .pipe(uglify("jquery.dynamiclist.templates.kendo.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest("scripts/package"));

    gulp.src("scripts/package/jquery.validate.unobtrusive.dynamic.js")
        .pipe(uglify("jquery.validate.unobtrusive.dynamic.min.js", { outSourceMap: true }))
        .pipe(eol())
        .pipe(gulp.dest("scripts/package"));
});