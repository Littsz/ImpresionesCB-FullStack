<template>
    <div class="container mt-5">
        <div class="card shadow-lg border-0">
            <div class="card-header bg-dark text-white text-center py-3">
                <h2 class="mb-0">📦 Sistema de Impresión CB</h2>
            </div>
            <div class="card-body bg-light text-center p-5">

                <div class="d-flex justify-content-center gap-4 mb-5">
                    <button class="btn btn-primary btn-lg px-4 shadow-sm" @click="$refs.fileInput.click()">
                        <i class="bi bi-file-earmark-spreadsheet"></i> 📂 Abrir Archivo
                    </button>
                    <input type="file" ref="fileInput" hidden @change="leerExcel" accept=".xlsx, .xls, .csv">

                    <button class="btn btn-outline-secondary btn-lg px-4 shadow-sm" data-bs-toggle="modal" data-bs-target="#configModal">
                        <i class="bi bi-gear"></i> ⚙️ Configurar Etiqueta
                    </button>
                </div>

                <div v-if="inventario.length > 0" class="animate__animated animate__fadeIn">
                    <h4 class="text-secondary text-start mb-3">Números de Inventario detectados:</h4>
                    <div class="table-responsive shadow-sm rounded">
                        <table class="table table-hover table-striped mb-0">
                            <thead class="table-primary">
                                <tr>
                                    <th class="py-3">#</th>
                                    <th class="py-3">Código detectado</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in inventario" :key="index">
                                    <td class="fw-bold">{{ index + 1 }}</td>
                                    <td>{{ item }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <button @click="imprimirEtiquetas" class="btn btn-success btn-lg mt-4 w-100 py-3 fw-bold shadow">
                        <i class="bi bi-printer"></i> 🖨️ GENERAR Y ENVIAR A IMPRIMIR
                    </button>
                </div>

                <div v-else class="text-muted py-5">
                    <p class="h5">Carga un archivo de Excel para comenzar...</p>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="configModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow">
                <div class="modal-header bg-secondary text-white">
                    <h5 class="modal-title">Ajustes de Impresión</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-3 text-start">
                        <label class="form-label fw-bold">TÍTULO (Línea 1):</label>
                        <input v-model="config.titulo" class="form-control form-control-lg" placeholder="Ej: PROPIEDAD DE CB">
                    </div>
                    <div class="mb-3 text-start">
                        <label class="form-label fw-bold">SUBTÍTULO (Línea 2):</label>
                        <input v-model="config.subtitulo" class="form-control form-control-lg" placeholder="Ej: DEPTO. SISTEMAS">
                    </div>
                    <div class="mb-3 text-start">
                        <label class="form-label fw-bold">PRE-ETIQUETA:</label>
                        <input v-model="config.preEtiqueta" class="form-control form-control-lg" placeholder="Ej: CB-INV-">
                    </div>
                </div>
                <div class="modal-footer border-0">
                    <button class="btn btn-primary w-100 py-2 fw-bold" @click="guardarConfig" data-bs-dismiss="modal">
                        Guardar y Aplicar
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue';
    import * as XLSX from 'xlsx';
    import axios from 'axios';

    const inventario = ref([]);
    const config = ref({ titulo: '', subtitulo: '', preEtiqueta: '' });

    const leerExcel = (event) => {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = (e) => {
            const data = new Uint8Array(e.target.result);
            const workbook = XLSX.read(data, { type: 'array' });
            const sheetName = workbook.SheetNames[0];
            const sheet = workbook.Sheets[sheetName];
            const rows = XLSX.utils.sheet_to_json(sheet, { header: 1 });

            // Limpiamos los datos para que sean strings puros
            inventario.value = rows.flat()
                .filter(cell => cell != null && cell.toString().trim() !== "")
                .map(cell => cell.toString().trim());
        };
        reader.readAsArrayBuffer(file);
    };

    const guardarConfig = () => {
        console.log("Configuración actualizada:", config.value);
    };

    const imprimirEtiquetas = async () => {
        if (inventario.value.length === 0) {
            alert("Primero carga un archivo de Excel.");
            return;
        }

        try {
            // Asegúrate de que el puerto 7288 sea el que sale en tu consola de C#
            const url = "https://localhost:7288/api/Impresion/generar-pdf";
            const respuesta = await axios.post(url, inventario.value);
            alert(respuesta.data.mensaje);
        } catch (error) {
            console.error("Error:", error);
            alert("Error al conectar con la API. Revisa que el puerto sea el 7288.");
        }
    };
</script>

<style>
    body {
        background-color: #f0f2f5;
    }

    .card {
        border-radius: 15px;
    }
</style>