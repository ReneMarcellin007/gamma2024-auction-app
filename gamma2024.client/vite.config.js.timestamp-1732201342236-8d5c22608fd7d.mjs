// vite.config.js
import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "file:///C:/Users/marca/Desktop/Gamma2024/gamma2024.client/node_modules/vite/dist/node/index.js";
import plugin from "file:///C:/Users/marca/Desktop/Gamma2024/gamma2024.client/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import fs from "fs";
import path from "path";
import child_process from "child_process";
var __vite_injected_original_import_meta_url = "file:///C:/Users/marca/Desktop/Gamma2024/gamma2024.client/vite.config.js";
var baseFolder = process.env.APPDATA !== void 0 && process.env.APPDATA !== "" ? `${process.env.APPDATA}/ASP.NET/https` : `${process.env.HOME}/.aspnet/https`;
var certificateArg = process.argv.map((arg) => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
var certificateName = certificateArg ? certificateArg.groups.value : "gamma2024.client";
if (!certificateName) {
  console.error(
    "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
  );
  process.exit(-1);
}
var certFilePath = path.join(baseFolder, `${certificateName}.pem`);
var keyFilePath = path.join(baseFolder, `${certificateName}.key`);
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (0 !== child_process.spawnSync(
    "dotnet",
    [
      "dev-certs",
      "https",
      "--export-path",
      certFilePath,
      "--format",
      "Pem",
      "--no-password"
    ],
    { stdio: "inherit" }
  ).status) {
    throw new Error("Could not create certificate.");
  }
}
var vite_config_default = defineConfig(({ mode }) => {
  return {
    plugins: [plugin()],
    //base: mode == "production" ? "/2162067" : "/",
    resolve: {
      alias: {
        "@": fileURLToPath(new URL("./src", __vite_injected_original_import_meta_url))
      }
    },
    server: {
      proxy: {
        "^/weatherforecast": {
          target: "https://localhost:7205/",
          secure: false
        }
      },
      port: 5173,
      https: {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath)
      }
    }
  };
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcuanMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxtYXJjYVxcXFxEZXNrdG9wXFxcXEdhbW1hMjAyNFxcXFxnYW1tYTIwMjQuY2xpZW50XCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxtYXJjYVxcXFxEZXNrdG9wXFxcXEdhbW1hMjAyNFxcXFxnYW1tYTIwMjQuY2xpZW50XFxcXHZpdGUuY29uZmlnLmpzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9DOi9Vc2Vycy9tYXJjYS9EZXNrdG9wL0dhbW1hMjAyNC9nYW1tYTIwMjQuY2xpZW50L3ZpdGUuY29uZmlnLmpzXCI7aW1wb3J0IHsgZmlsZVVSTFRvUGF0aCwgVVJMIH0gZnJvbSBcIm5vZGU6dXJsXCI7XHJcblxyXG5pbXBvcnQgeyBkZWZpbmVDb25maWcgfSBmcm9tIFwidml0ZVwiO1xyXG5pbXBvcnQgcGx1Z2luIGZyb20gXCJAdml0ZWpzL3BsdWdpbi12dWVcIjtcclxuaW1wb3J0IGZzIGZyb20gXCJmc1wiO1xyXG5pbXBvcnQgcGF0aCBmcm9tIFwicGF0aFwiO1xyXG5pbXBvcnQgY2hpbGRfcHJvY2VzcyBmcm9tIFwiY2hpbGRfcHJvY2Vzc1wiO1xyXG5cclxuY29uc3QgYmFzZUZvbGRlciA9XHJcbiAgICBwcm9jZXNzLmVudi5BUFBEQVRBICE9PSB1bmRlZmluZWQgJiYgcHJvY2Vzcy5lbnYuQVBQREFUQSAhPT0gXCJcIlxyXG4gICAgICAgID8gYCR7cHJvY2Vzcy5lbnYuQVBQREFUQX0vQVNQLk5FVC9odHRwc2BcclxuICAgICAgICA6IGAke3Byb2Nlc3MuZW52LkhPTUV9Ly5hc3BuZXQvaHR0cHNgO1xyXG5cclxuY29uc3QgY2VydGlmaWNhdGVBcmcgPSBwcm9jZXNzLmFyZ3ZcclxuICAgIC5tYXAoKGFyZykgPT4gYXJnLm1hdGNoKC8tLW5hbWU9KD88dmFsdWU+LispL2kpKVxyXG4gICAgLmZpbHRlcihCb29sZWFuKVswXTtcclxuY29uc3QgY2VydGlmaWNhdGVOYW1lID0gY2VydGlmaWNhdGVBcmdcclxuICAgID8gY2VydGlmaWNhdGVBcmcuZ3JvdXBzLnZhbHVlXHJcbiAgICA6IFwiZ2FtbWEyMDI0LmNsaWVudFwiO1xyXG5cclxuaWYgKCFjZXJ0aWZpY2F0ZU5hbWUpIHtcclxuICAgIGNvbnNvbGUuZXJyb3IoXHJcbiAgICAgICAgXCJJbnZhbGlkIGNlcnRpZmljYXRlIG5hbWUuIFJ1biB0aGlzIHNjcmlwdCBpbiB0aGUgY29udGV4dCBvZiBhbiBucG0veWFybiBzY3JpcHQgb3IgcGFzcyAtLW5hbWU9PDxhcHA+PiBleHBsaWNpdGx5LlwiXHJcbiAgICApO1xyXG4gICAgcHJvY2Vzcy5leGl0KC0xKTtcclxufVxyXG5cclxuY29uc3QgY2VydEZpbGVQYXRoID0gcGF0aC5qb2luKGJhc2VGb2xkZXIsIGAke2NlcnRpZmljYXRlTmFtZX0ucGVtYCk7XHJcbmNvbnN0IGtleUZpbGVQYXRoID0gcGF0aC5qb2luKGJhc2VGb2xkZXIsIGAke2NlcnRpZmljYXRlTmFtZX0ua2V5YCk7XHJcblxyXG5pZiAoIWZzLmV4aXN0c1N5bmMoY2VydEZpbGVQYXRoKSB8fCAhZnMuZXhpc3RzU3luYyhrZXlGaWxlUGF0aCkpIHtcclxuICAgIGlmIChcclxuICAgICAgICAwICE9PVxyXG4gICAgICAgIGNoaWxkX3Byb2Nlc3Muc3Bhd25TeW5jKFxyXG4gICAgICAgICAgICBcImRvdG5ldFwiLFxyXG4gICAgICAgICAgICBbXHJcbiAgICAgICAgICAgICAgICBcImRldi1jZXJ0c1wiLFxyXG4gICAgICAgICAgICAgICAgXCJodHRwc1wiLFxyXG4gICAgICAgICAgICAgICAgXCItLWV4cG9ydC1wYXRoXCIsXHJcbiAgICAgICAgICAgICAgICBjZXJ0RmlsZVBhdGgsXHJcbiAgICAgICAgICAgICAgICBcIi0tZm9ybWF0XCIsXHJcbiAgICAgICAgICAgICAgICBcIlBlbVwiLFxyXG4gICAgICAgICAgICAgICAgXCItLW5vLXBhc3N3b3JkXCIsXHJcbiAgICAgICAgICAgIF0sXHJcbiAgICAgICAgICAgIHsgc3RkaW86IFwiaW5oZXJpdFwiIH1cclxuICAgICAgICApLnN0YXR1c1xyXG4gICAgKSB7XHJcbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKFwiQ291bGQgbm90IGNyZWF0ZSBjZXJ0aWZpY2F0ZS5cIik7XHJcbiAgICB9XHJcbn1cclxuXHJcbi8vIGh0dHBzOi8vdml0ZWpzLmRldi9jb25maWcvXHJcbmV4cG9ydCBkZWZhdWx0IGRlZmluZUNvbmZpZygoeyBtb2RlIH0pID0+IHtcclxuICAgIHJldHVybiB7XHJcbiAgICAgICAgcGx1Z2luczogW3BsdWdpbigpXSxcclxuICAgICAgICAvL2Jhc2U6IG1vZGUgPT0gXCJwcm9kdWN0aW9uXCIgPyBcIi8yMTYyMDY3XCIgOiBcIi9cIixcclxuICAgICAgICByZXNvbHZlOiB7XHJcbiAgICAgICAgICAgIGFsaWFzOiB7XHJcbiAgICAgICAgICAgICAgICBcIkBcIjogZmlsZVVSTFRvUGF0aChuZXcgVVJMKFwiLi9zcmNcIiwgaW1wb3J0Lm1ldGEudXJsKSksXHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgfSxcclxuICAgICAgICBzZXJ2ZXI6IHtcclxuICAgICAgICAgICAgcHJveHk6IHtcclxuICAgICAgICAgICAgICAgIFwiXi93ZWF0aGVyZm9yZWNhc3RcIjoge1xyXG4gICAgICAgICAgICAgICAgICAgIHRhcmdldDogXCJodHRwczovL2xvY2FsaG9zdDo3MjA1L1wiLFxyXG4gICAgICAgICAgICAgICAgICAgIHNlY3VyZTogZmFsc2UsXHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICBwb3J0OiA1MTczLFxyXG4gICAgICAgICAgICBodHRwczoge1xyXG4gICAgICAgICAgICAgICAga2V5OiBmcy5yZWFkRmlsZVN5bmMoa2V5RmlsZVBhdGgpLFxyXG4gICAgICAgICAgICAgICAgY2VydDogZnMucmVhZEZpbGVTeW5jKGNlcnRGaWxlUGF0aCksXHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgfSxcclxuICAgIH07XHJcbn0pO1xyXG4iXSwKICAibWFwcGluZ3MiOiAiO0FBQWlWLFNBQVMsZUFBZSxXQUFXO0FBRXBYLFNBQVMsb0JBQW9CO0FBQzdCLE9BQU8sWUFBWTtBQUNuQixPQUFPLFFBQVE7QUFDZixPQUFPLFVBQVU7QUFDakIsT0FBTyxtQkFBbUI7QUFOMkwsSUFBTSwyQ0FBMkM7QUFRdFEsSUFBTSxhQUNGLFFBQVEsSUFBSSxZQUFZLFVBQWEsUUFBUSxJQUFJLFlBQVksS0FDdkQsR0FBRyxRQUFRLElBQUksT0FBTyxtQkFDdEIsR0FBRyxRQUFRLElBQUksSUFBSTtBQUU3QixJQUFNLGlCQUFpQixRQUFRLEtBQzFCLElBQUksQ0FBQyxRQUFRLElBQUksTUFBTSxzQkFBc0IsQ0FBQyxFQUM5QyxPQUFPLE9BQU8sRUFBRSxDQUFDO0FBQ3RCLElBQU0sa0JBQWtCLGlCQUNsQixlQUFlLE9BQU8sUUFDdEI7QUFFTixJQUFJLENBQUMsaUJBQWlCO0FBQ2xCLFVBQVE7QUFBQSxJQUNKO0FBQUEsRUFDSjtBQUNBLFVBQVEsS0FBSyxFQUFFO0FBQ25CO0FBRUEsSUFBTSxlQUFlLEtBQUssS0FBSyxZQUFZLEdBQUcsZUFBZSxNQUFNO0FBQ25FLElBQU0sY0FBYyxLQUFLLEtBQUssWUFBWSxHQUFHLGVBQWUsTUFBTTtBQUVsRSxJQUFJLENBQUMsR0FBRyxXQUFXLFlBQVksS0FBSyxDQUFDLEdBQUcsV0FBVyxXQUFXLEdBQUc7QUFDN0QsTUFDSSxNQUNBLGNBQWM7QUFBQSxJQUNWO0FBQUEsSUFDQTtBQUFBLE1BQ0k7QUFBQSxNQUNBO0FBQUEsTUFDQTtBQUFBLE1BQ0E7QUFBQSxNQUNBO0FBQUEsTUFDQTtBQUFBLE1BQ0E7QUFBQSxJQUNKO0FBQUEsSUFDQSxFQUFFLE9BQU8sVUFBVTtBQUFBLEVBQ3ZCLEVBQUUsUUFDSjtBQUNFLFVBQU0sSUFBSSxNQUFNLCtCQUErQjtBQUFBLEVBQ25EO0FBQ0o7QUFHQSxJQUFPLHNCQUFRLGFBQWEsQ0FBQyxFQUFFLEtBQUssTUFBTTtBQUN0QyxTQUFPO0FBQUEsSUFDSCxTQUFTLENBQUMsT0FBTyxDQUFDO0FBQUE7QUFBQSxJQUVsQixTQUFTO0FBQUEsTUFDTCxPQUFPO0FBQUEsUUFDSCxLQUFLLGNBQWMsSUFBSSxJQUFJLFNBQVMsd0NBQWUsQ0FBQztBQUFBLE1BQ3hEO0FBQUEsSUFDSjtBQUFBLElBQ0EsUUFBUTtBQUFBLE1BQ0osT0FBTztBQUFBLFFBQ0gscUJBQXFCO0FBQUEsVUFDakIsUUFBUTtBQUFBLFVBQ1IsUUFBUTtBQUFBLFFBQ1o7QUFBQSxNQUNKO0FBQUEsTUFDQSxNQUFNO0FBQUEsTUFDTixPQUFPO0FBQUEsUUFDSCxLQUFLLEdBQUcsYUFBYSxXQUFXO0FBQUEsUUFDaEMsTUFBTSxHQUFHLGFBQWEsWUFBWTtBQUFBLE1BQ3RDO0FBQUEsSUFDSjtBQUFBLEVBQ0o7QUFDSixDQUFDOyIsCiAgIm5hbWVzIjogW10KfQo=
