// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

window.setImage = async (imageElementId, imageStream) => {
  const arrayBuffer = await imageStream.arrayBuffer();
  const blob = new Blob([arrayBuffer]);
  const url = URL.createObjectURL(blob);
  const image = document.getElementById(imageElementId);
  image.onload = () => {
    URL.revokeObjectURL(url);
  }
  image.src = url;
}

window.draw = async (canvasReference, imageStream, width, height) => {
    const bmp = await imageStream.arrayBuffer();
    const ctx = canvasReference.getContext('2d');
    const imageData = ctx.createImageData(width, height);
    imageData.data.set(bmp);
    ctx.putImageData(imageData, 0, 0);
}