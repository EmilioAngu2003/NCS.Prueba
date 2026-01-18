export function inicializarGestionDetalles(config) {
    const {
        accordionId = '#facturaAccordion',
        btnAgregarId = '#agregarLineaBtn',
        templateId = '#lineaFacturaTemplate'
    } = config;

    function getNextIndex() {
        let maxIndex = -1;
        $(accordionId).find('[data-index]').each(function () {
            const currentIndex = parseInt($(this).data('index'));
            if (!isNaN(currentIndex) && currentIndex > maxIndex) {
                maxIndex = currentIndex;
            }
        });
        return maxIndex + 1;
    }

    // Actualizar encabezado dinámicamente
    $(document).on('input', 'input[name$=".Concepto"]', function () {
        const newConcepto = $(this).val();
        const $lineaContainer = $(this).closest('[data-index]');

        $lineaContainer.find('[data-concepto-display]').text(newConcepto || "Nueva Línea");
        $lineaContainer.find('input[name$=".Concepto"]').not(this).val(newConcepto);
    });

    // Botón Agregar
    $(btnAgregarId).click(function () {
        const nextIndex = getNextIndex();
        const templateHtml = $(templateId).html();
        const newRowHtml = templateHtml.replace(/__INDEX__/g, nextIndex);

        const $newRow = $(newRowHtml).appendTo(accordionId);

        const $form = $("form");
        $form.removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($form);

        $newRow.find('input[type="text"]').first().focus();
    });

    // Botón Eliminar
    $(accordionId).on('click', '.eliminar-linea', function (e) {
        e.preventDefault();
        const $fila = $(this).closest('[data-index]');
        if (confirm("¿Estás seguro de que quieres eliminar esta línea de factura?")) {
            $fila.remove();
        }
    });
}