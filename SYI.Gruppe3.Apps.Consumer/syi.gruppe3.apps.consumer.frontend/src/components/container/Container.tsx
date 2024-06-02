/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import 'bootstrap/dist/css/bootstrap.min.css';
import Chart2 from '../Chart2';
import { useState } from 'react';
import { Models } from '../../models/models';

function Container() {
    const [filterModel, setFilterModel] = useState<Models.IFilterModel | undefined>();

  

    const generateYearOptions = () => {
        const arr = [];

        const startYear = 2012;
        const endYear = new Date().getFullYear();

        for (let i = endYear; i >= startYear; i--) {
            arr.push(<option value={i}>{i}</option>);
        }

        return arr;
    };

    return (
        <div>
            <div style={{ width: '100%', height: '100%', display: 'inline-flex', padding: '10px', margin: '5px' }}>
                <label style={{ paddingRight: '50px' }}>
                    <span style={{ paddingRight: '5px' }}>
                        Deadly Only
                    </span>


                    <input type="checkbox" value="true" checked={filterModel?.deadlyOnly}
                        onChange={() => {
                            let currentState: boolean | undefined;
                            if (filterModel !== undefined)
                                currentState = filterModel.deadlyOnly;
                            setFilterModel({
                                deadlyOnly: !currentState,
                                borough: filterModel?.borough,
                                yearFrom: filterModel?.yearFrom,
                                yearTo: filterModel?.yearTo
                            });
                        }
                        }>
                    </input>
                </label>
                <label style={{ paddingRight: '5px' }}>State Filter:</label>
                <select style={{ paddingRight: '5px' }} onChange={(event: any) => {
                    const value = event.target.value;
                    setFilterModel({
                        deadlyOnly: filterModel?.deadlyOnly,
                        borough: value,
                        yearFrom: filterModel?.yearFrom,
                        yearTo: filterModel?.yearTo
                    })
                }} value={filterModel?.borough}>
                    <option value="">
                    </option>
                    <option value="BRONX">
                        BRONX
                    </option>
                    <option value="BROOKLYN">
                        BROOKLYN
                    </option>
                    <option value="QUEENS">
                        QUEENS
                    </option>
                    <option value="MANHATTAN">
                        MANHATTAN
                    </option>
                    <option value="STATEN ISLAND">
                        STATEN ISLAND
                    </option>
                </select>
                <label style={{ paddingRight: '5px' }} style={{ paddingRight: '5px' }}>Year From:</label>
                <select style={{ paddingRight: '5px' }} onChange={(event: any) => {
                    const value = event.target.value;
                    setFilterModel({
                        deadlyOnly: filterModel?.deadlyOnly,
                        borough: filterModel?.borough,
                        yearFrom: value,
                        yearTo: filterModel?.yearTo
                    })
                }} value={filterModel?.yearFrom}>
                    <option value=''></option>
                    {generateYearOptions()}
                </select>
                <label style={{ paddingRight: '5px' }}>Year To:</label>
                <select onChange={(event: any) => {
                    const value = event.target.value;
                    setFilterModel({
                        deadlyOnly: filterModel?.deadlyOnly,
                        borough: filterModel?.borough,
                        yearFrom: filterModel?.yearFrom,
                        yearTo: value
                    })
                }} value={filterModel?.yearTo}>
                    <option value=''></option>
                    {generateYearOptions()}
                </select>
            </div>
            <div style={{ width: '100%', height: '100%', display: 'inline-flex', padding: '5px', margin: '5px' }}>
                <div style={{ width: '100%', height: '100%', border: 'solid 1px black', padding: '5px', margin: '5px' }}>
                    <Chart2 filter={
                        filterModel
                    }>
                    </Chart2>
                </div>
            </div>
        </div>);

}

export default Container;