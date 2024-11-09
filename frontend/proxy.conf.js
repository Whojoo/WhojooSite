module.exports = {
  "/api": {
    target:
      process.env["services__api-gateway__https__0"] ||
      process.env["services__api-gateway__http__0"],
    secure: process.env["NODE_ENV"] !== "development",
    pathRewrite: {
      "^/api": "",
    },
  },
};
