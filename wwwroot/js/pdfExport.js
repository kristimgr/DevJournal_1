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

    // Print logic
    iframe.contentWindow.focus();
    iframe.contentWindow.print();

    // Cleanup (delayed to allow print dialog)
    setTimeout(() => {
        document.body.removeChild(iframe);
    }, 1000);
};

window.saveFile = (filename, contentType, content) => {
    // Create a Blob with the content
    const blob = new Blob([content], { type: contentType });

    // Create a temporary anchor element
    const a = document.createElement('a');
    const url = URL.createObjectURL(blob);

    a.href = url;
    a.download = filename;
    a.style.display = 'none';

    document.body.appendChild(a);
    a.click();

    // Cleanup
    setTimeout(() => {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }, 1000);
};

window.downloadPdf = (filename, content) => {
    // Create a temporary container for the HTML content
    var container = document.createElement('div');
    container.innerHTML = content;
    container.style.width = '800px'; // typical A4 width at 96dpi is ~794px
    container.style.padding = '20px';
    container.style.background = 'white';

    // We need to append it to body to render, but hide it? 
    // html2pdf can render off-screen but it's safer to have it in DOM.
    // However, html2pdf(element) works best.

    // Let's create a better structure
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
        margin: [10, 10], // top, left, bottom, right
        filename: filename,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2, useCORS: true },
        jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
    };

    // New Promise-based usage of html2pdf
    if (typeof html2pdf !== 'undefined') {
        html2pdf().set(opt).from(element).save();
    } else {
        console.error("html2pdf library not loaded!");
        alert("Error: PDF library not loaded. Please refresh the page.");
    }
};
