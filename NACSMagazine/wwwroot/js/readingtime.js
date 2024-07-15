function readingTime() {
  const text = document.getElementById("article").innerText;
  alert(text);
  const wpm = 225;
  const words = text.trim().split(/\s+/).length;
  const time = Math.ceil(words / wpm);
  document.getElementById("time").innerText = time;
}
readingTime();