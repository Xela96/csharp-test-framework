import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

export const options = {
    vus: 10,
    iterations: 20,
}

export default function () {
    const res = http.get('https://dohertyalex.cc/')

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(1);
}