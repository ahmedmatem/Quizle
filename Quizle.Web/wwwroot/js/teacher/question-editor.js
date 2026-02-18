(function () {
    const typeSel = document.getElementById('qType');
    const mcBox = document.getElementById('mcBox');
    const numBox = document.getElementById('numBox');
    const txtBox = document.getElementById('txtBox');
    const opts = document.getElementById('opts');
    const addBtn = document.getElementById('btnAddOpt');

    function showByType() {
        const t = typeSel.value;
        mcBox.style.display = (t == 'MultipleChoice') ? '' : 'none';
        numBox.style.display = (t == 'Numeric') ? '' : 'none';
        txtBox.style.display = (t == 'ShortText') ? '' : 'none';
    }

    function renumber() {
        const rows = opts.querySelectorAll('.opt-row');
        rows.forEach((row, i) => {
            row.setAttribute('data-index', i);

            // hidden id
            row.querySelector('input[type="hidden"]').setAttribute('name', `Options[${i}].Id`);

            // text
            row.querySelector('input.form-control').setAttribute('name', `Options[${i}].Text`);

            // radio
            row.querySelector('input[type="radio"]').setAttribute('value', i);
        });
    }

    addBtn?.addEventListener('click', () => {
        const i = opts.querySelectorAll('.opt-row').length;

        const div = document.createElement('div');
        div.className = 'row g-2 align-items-center mb-2 opt-row';
        div.setAttribute('data-index', i);

        div.innerHTML = `
    <input type="hidden" name="Options[${i}].Id" value="" />
    <div class="col-1 text-center">
        <input class="form-check-input" type="radio" name="CorrectIndex" value="${i}" />
    </div>
    <div class="col-9">
        <input class="form-control" name="Options[${i}].Text" value="" placeholder="Option text..." />
    </div>
    <div class="col-2 text-end">
        <button type="button" class="btn btn-outline-danger btn-sm btnRemove">Remove</button>
    </div>
    `;
        opts.appendChild(div);
    });

    opts?.addEventListener('click', (e) => {
        const btn = e.target.closest('.btnRemove');
        if (!btn) return;

        const row = btn.closest('.opt-row');
        row.remove();
        renumber();

        // ensure at least one correct selected if possible
        const radios = opts.querySelectorAll('input[type="radio"]');
        if (radios.length && !opts.querySelector('input[type="radio"]:checked')) {
            radios[0].checked = true;
        }
    });

    typeSel?.addEventListener('change', showByType);
    showByType();
    renumber();
})();
