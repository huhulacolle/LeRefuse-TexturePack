import { useEffect, useState } from "react";

interface FileModel {
  fileName: string;
  url: string;
}

function App() {
  const [paints, setPaints] = useState<FileModel[]>([]);

  useEffect(() => {
    getPaints();
  }, []);

  return (
    <div>
      <h1>Paints</h1>
      {paints.length === 0 ? (
        <p>Chargement</p>
      ) : (
        <div style={{ display: "grid", gap: "1rem" }}>
          {paints.map((paint) => (
            <div key={paint.fileName}>
              <h3>{paint.fileName}</h3>
              <img src={paint.url} />
              <div>clique sur changer l'image</div>
              <input
                type="file"
                accept="image/*"
                onChange={(e) => upload(e, paint.fileName)}
              />
            </div>
          ))}
        </div>
      )}
    </div>
  );

  async function upload(
    e: React.ChangeEvent<HTMLInputElement>,
    fileName: string,
  ) {
    const file = e.target.files?.[0];

    if (!file) return;

    const formData = new FormData();
    formData.append("file", file);

    await fetch(`/api/Painting/${fileName}`, {
      method: "POST",
      body: formData,
    });

    alert(`image ${fileName} uploadé`)

    await getPaints();
  }

  async function getPaints() {
    const data = await fetch("/api/Painting");
    const result: FileModel[] = await data.json();
    setPaints(result);
  }
}

export default App;
