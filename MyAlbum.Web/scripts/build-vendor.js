const fs = require("fs");
const path = require("path");

function ensureDir(dir) {
    fs.mkdirSync(dir, { recursive: true });
}

function copy(src, dest) {
    ensureDir(path.dirname(dest));
    fs.copyFileSync(src, dest);
    console.log(`copy: ${dest}`);
}

const root = process.cwd();

const nm = (p) => path.join(root, "node_modules", p);
const out = (p) => path.join(root, "wwwroot", "vendor", p);

// bootstrap
copy(nm("bootstrap/dist/css/bootstrap.min.css"), out("bootstrap/bootstrap.min.css"));
copy(nm("bootstrap/dist/js/bootstrap.bundle.min.js"), out("bootstrap/bootstrap.bundle.min.js"));

// jquery
copy(nm("jquery/dist/jquery.min.js"), out("jquery/jquery.min.js"));

// jquery-validation
copy(nm("jquery-validation/dist/jquery.validate.min.js"), out("jquery-validation/jquery.validate.min.js"));

// jquery-validation-unobtrusive
copy(
    nm("jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js"),
    out("jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js")
);

console.log("done.");