const PROXY_CONFIG = [
  {
    context: [
      "/Chess",
    ],
    target: "https://localhost:7024",
    secure: true
  }
]

module.exports = PROXY_CONFIG;
