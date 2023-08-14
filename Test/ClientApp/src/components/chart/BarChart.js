import React, { useMemo } from 'react';

import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    PointElement,
    Title,
    Tooltip,
    Legend,
    Filler,
    BarElement,
    ArcElement,
} from 'chart.js';

import { Bar } from 'react-chartjs-2';

ChartJS.register(
    CategoryScale,
    LinearScale,
    PointElement,
    BarElement,
    Title,
    Tooltip,
    Legend,
    Filler,
    ArcElement
);

const BarChart = ({ data }) => {

    const names = []
    const values = []
    const colors = []

    const rowData = data?.rows;

    for (let i in rowData) {
        names.push(rowData[i].name)
        values.push(rowData[i].value)
        colors.push(`#${rowData[i].color}`)
    }

        const options = {
            fill: true,
            animation: true,
            responsive: true,
            hoverOffset: 4,
    };

    const dataChart = useMemo(() => {
        const labels = names;
        return {
            datasets: [
                {
                    label:"Value",
                    data: values,
                    backgroundColor: colors,
                    borderWidth: 1,
                }
            ],
            labels,
        };
    }, [data]);

    return <Bar data={dataChart} options={options} />;

}

export default BarChart;