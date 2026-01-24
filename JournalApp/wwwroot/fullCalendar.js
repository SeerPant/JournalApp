let calendar;

window.initializeCalendar = (events) => {
    var calendarEl = document.getElementById('calendar');

    calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth'
        },
        events: events,
        eventColor: '#38182F',
        eventTextColor: '#FFFFFF',
        height: 'auto',
        eventClick: function (info) {

        }
    });

    calendar.render();
};

window.updateCalendarEvents = (events) => {
    if (calendar) {
        calendar.removeAllEvents();
        calendar.addEventSource(events);
    }
};

window.destroyCalendar = () => {
    if (calendar) {
        calendar.destroy();
    }
};