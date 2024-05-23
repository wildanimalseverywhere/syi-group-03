import 'bootstrap/dist/css/bootstrap.min.css';
import { ApiClient } from '../../infrastructure/apiClient';
function Table(props: { data: ApiClient.DataResponseModel | undefined }) {

    return (
        (props === undefined || props.data === undefined || props.data.data === undefined) ? null :
            props.data.data.length === 0 ?
                <p>No data to display</p>
                :
                <div className="row">
                    <div className="col-12">
                        <table className="table table-responsive" style={{ tableLayout: 'fixed', width: '100%', fontSize: '12px', wordWrap:'break-word' }}>
                            <thead>
                                <tr>

                                    {
                                        Object.keys(props.data.data[0]).map(t =>
                                            <th style={{ border: '0.5px solid black' }}>
                                                {t}
                                            </th>)
                                    }

                                </tr>
                            </thead>
                            <tbody>
                                {
                                    props.data.data.map(t =>
                                        <tr>
                                            {
                                                Object.values(t).map(f => <td> {f}</td>)
                                            }
                                        </tr>)
                                }
                            </tbody>
                        </table>
                    </div>
                </div>

    );

}

export default Table;