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
