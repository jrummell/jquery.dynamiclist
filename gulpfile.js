/// <binding />
const gulp = require("gulp");
const terser = require("gulp-terser");
const rename = require("gulp-rename");
const sourcemaps = require("gulp-sourcemaps");
const eol = require("gulp-eol");

const dist = "dist";

function minify(src, minName) {
    return function () {
        return gulp
            .src(src)
            .pipe(sourcemaps.init())
            .pipe(terser())
            .pipe(rename(minName))
            .pipe(sourcemaps.write("."))
            .pipe(eol())
            .pipe(gulp.dest(dist));
    };
}

gulp.task(
    "default",
    gulp.parallel(
        minify("src/jquery.dynamiclist.js", "jquery.dynamiclist.min.js"),
        minify(
            "src/jquery.dynamiclist.templates.bootstrap.js",
            "jquery.dynamiclist.templates.bootstrap.min.js"
        ),
        minify(
            "src/jquery.dynamiclist.templates.kendo.js",
            "jquery.dynamiclist.templates.kendo.min.js"
        ),
        minify(
            "src/jquery.validate.unobtrusive.dynamic.js",
            "jquery.validate.unobtrusive.dynamic.min.js"
        )
    )
);


