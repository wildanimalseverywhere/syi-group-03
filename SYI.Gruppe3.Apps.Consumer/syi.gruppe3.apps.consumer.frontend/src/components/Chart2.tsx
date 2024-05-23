/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import Plot from 'react-plotly.js';
import Table from './table/Table';
import Panel from './panel/Panel';
import { useEffect, useState } from 'react';
import { ApiClient } from '../infrastructure/apiClient';
import { Models } from '../models/models';
function Chart2(props: { filter: Models.IFilterModel }) {


    function groupBy(list: { [key: string]: any }[], keyGetter: any) {
        const map = new Map();
        list.forEach((item) => {
            const key = keyGetter(item);
            const collection = map.get(key);
            if (!collection) {
                map.set(key, [item]);
            } else {
                collection.push(item);
            }
        });
        return map;
    }


    const [queryData, setQueryData] = useState<ApiClient.DataResponseModel | undefined>();

    let chartSqlQuery = "select Borough, extract('YEAR' from Crashdate) as Year,";

    if (props !== undefined && props.filter !== undefined && props.filter.deadlyOnly === true) {
        chartSqlQuery = chartSqlQuery + " fsum(NumberOfPersonsKilled) as Count ";
    }
    else {
        chartSqlQuery = chartSqlQuery + " fsum(NumberOfPersonsInjured) as Count  ";
    }
    chartSqlQuery = chartSqlQuery + " from data";

    if (props !== undefined && props.filter !== undefined) {
        if (props.filter.borough !== undefined && props.filter.borough !== '') {
            chartSqlQuery = chartSqlQuery + " where Borough == '" + props.filter.borough + "'";
        }
    }

    chartSqlQuery = chartSqlQuery + " group by Year, Borough";


    chartSqlQuery = "select * from (" + chartSqlQuery + ")";

    useEffect(() => {
        async function fetchData() {
            const client = new ApiClient.Client();
            const query = new ApiClient.DataQueryModel();
            query.sqlQuery = chartSqlQuery;
            query.orderBy = "Year";
            query.limitOverride = 4999;
            const rawResponse = await client.query(query);
            setQueryData(rawResponse);
        }
        fetchData();
    }, [chartSqlQuery]);

    const yAll = Array<any>();

    if (queryData !== undefined && queryData.data !== undefined) {

        const affectedArray = groupBy(queryData?.data, t => t['Borough']);

        for (const [key, value] of affectedArray.entries()) {
            const years = value.map(f => new Date(f['Year'], 1, 1)).sort((a: Date, b: Date): number => a.getTime() - b.getTime())
            yAll.push({
                type: "scatter",
                mode: "lines",
                name: key,
                x: years,
                y: value.map(f => f['Count']),
                line: {
                    color: "#000000".replace(/0/g, function () { return (~~(Math.random() * 16)).toString(16) })
                }
            })
        }


    }


    const data = yAll;

    const layout = {
        title: 'Injury / Kill History per Borough',
        xaxis: {
            autorange: true,
            range: ['2012-01-01', '2024-01-01'],
            rangeselector: {
                buttons: [
                    {
                        count: 1,
                        label: '1m',
                        step: 'month',
                        stepmode: 'backward'
                    },
                    {
                        count: 6,
                        label: '6m',
                        step: 'month',
                        stepmode: 'backward'
                    },
                    { step: 'all' }
                ]
            },
            rangeslider: { range: ['2012-01-01', '2024-01-01'] },
            type: 'date'
        },
        yaxis: {
            autorange: true,
            range: [86.8700008333, 138.870004167],
            type: 'linear'
        }
    };



    return (
        <div>
            <Plot
                data={data}
                layout={layout}
            ></Plot>
            <Panel data={queryData}>
            </Panel>
            <Table data={queryData}>
            </Table>

        </div>

    );

}

export default Chart2;