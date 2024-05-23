import Plot from 'react-plotly.js';
import Table from './table/Table';
import Panel from './panel/Panel';
import { useEffect, useState } from 'react';
import { ApiClient } from '../infrastructure/apiClient';
import { Models } from '../models/models';
function Chart1(props: { filter: Models.IFilterModel }) {
    const [queryData, setQueryData] = useState<ApiClient.DataResponseModel | undefined>();

    let deadlyClause = "";
    let boroughClause = "";

    console.log(props);

    if (props !== undefined && props.filter !== undefined) {
        if (props.filter.deadlyOnly === true)
            deadlyClause = " having Killed > 1";
        if (props.filter.borough !== undefined && props.filter.borough !== '')
            boroughClause = "where Borough == '" + props.filter.borough + "'";
    }


    let chartSqlQuery = "select * from (select Borough, longitude, latitude, fsum(NumberOfPersonsInjured) as Injured, fsum(NumberOfPersonsKilled) as Killed from data ";

    if (boroughClause !== "")
        chartSqlQuery = chartSqlQuery + boroughClause;
    chartSqlQuery = chartSqlQuery + " group by longitude,latitude, Borough ";
    if (deadlyClause !== "")
        chartSqlQuery = chartSqlQuery + deadlyClause;
    else
        chartSqlQuery = chartSqlQuery + " having Injured > 1";
    chartSqlQuery = chartSqlQuery + ")";
    



    useEffect(() => {
        setTimeout(async () => {
            const client = new ApiClient.Client();
            const query = new ApiClient.DataQueryModel();
            query.sqlQuery = chartSqlQuery;
            const rawResponse = await client.query(query);
            setQueryData(rawResponse);
        }, 250)
    }, [chartSqlQuery]);

    const cityLat = [];
    const cityLon = [];
    const hoverText = [];
    const citySize = [];

    if (queryData !== undefined && queryData.data !== undefined) {
        for (let i = 0; i < queryData?.data.length; i++) {
            const currentRow = queryData.data[i];

            const currentSize = (currentRow['Injured'] + currentRow['Killed']);
            const currentText = "Injured: " + currentRow['Injured'] + " Killed: " + currentRow['Killed'];
            cityLat.push(currentRow['Latitude']);
            cityLon.push(currentRow['Longitude']);
            citySize.push(currentSize);
            hoverText.push(currentText);
        }
    }



    const data = [{
        type: 'scattergeo',
        lat: cityLat,
        lon: cityLon,
        hoverinfo: 'text',
        text: hoverText,
        marker: {
            size: citySize,
            line: {
                color: 'black',
                width: 2
            },
        }
    }];

    const layout = {
        title: 'NYPD Injured and Killed',
        showlegend: false,
        autosize: false,
        width: 500,
        height: 500,
        geo: {
            scope: 'usa',
            showlakes: true,
            lakecolor: 'rgb(255,255,255)',
            center: { lon: -73.975592, lat: 40.7578587 },

            lonaxis: {
                range: [-75, -70]
            },
            lataxis: {
                range: [40, 44]
            },
            projection: {
                type: 'albers usa'
            },
            showland: true,

        },
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

export default Chart1;