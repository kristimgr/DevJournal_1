window.printPdf = (title, content) => {
    // Create a hidden iframe
    var iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    document.body.appendChild(iframe);

    var doc = iframe.contentWindow.document;
    doc.open();
    doc.write(`
        <html>
        <head>
            <title>${title}</title>
            <style>
                body { font-family: sans-serif; padding: 20px; }
                h1 { color: #2c5282; }
                .entry { margin-bottom: 30px; border-bottom: 1px solid #ccc; padding-bottom: 20px; }
                .meta { color: #666; font-size: 0.9em; margin-bottom: 10px; }
                .badge { background: #eee; padding: 2px 6px; border-radius: 4px; font-size: 0.8em; margin-right: 5px; }
            </style>
        </head>
        <body>
            ${content}
        </body>
        </html>
    `);
    doc.close();

    iframe.contentWindow.focus();
    iframe.contentWindow.print();

    setTimeout(() => {
        document.body.removeChild(iframe);
    }, 1000);
};

window.saveFile = (filename, contentType, content) => {
    const blob = new Blob([content], { type: contentType });

    const a = document.createElement('a');
    const url = URL.createObjectURL(blob);

    a.href = url;
    a.download = filename;
    a.style.display = 'none';

    document.body.appendChild(a);
    a.click();

    setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 1000);
};

window.downloadPdf = (filename, content) => {
    var container = document.createElement('div');
    container.innerHTML = content;
    container.style.width = '800px'; // typical A4 width at 96dpi is ~794px
    container.style.padding = '20px';
    container.style.background = 'white';

    var element = document.createElement('div');
    element.innerHTML = `
        <style>
            body { font-family: 'Helvetica', sans-serif; color: #333; }
            h1 { color: #2c5282; border-bottom: 2px solid #eee; padding-bottom: 10px; }
            .entry { margin-bottom: 30px; page-break-inside: avoid; border: 1px solid #eee; padding: 15px; border-radius: 5px; }
            .meta { color: #666; font-size: 0.9em; margin-bottom: 5px; }
        </style>
        ${content}
    `;

    var opt = {
        margin: [10, 10],
        filename: filename,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2, useCORS: true },
        jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };
    
    if (typeof html2pdf !== 'undefined') {
        html2pdf().set(opt).from(element).save();
    } else {
        console.error("html2pdf library not loaded!");
        alert("Error: PDF library not loaded. Please refresh the page.");
    }
};
