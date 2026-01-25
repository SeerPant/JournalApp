let calendar;
window.initializeCalendar = (events) => {
    console.log('initializeCalendar called');
    console.log('Events received:', events);
    console.log('Number of events:', events ? events.length : 'events is null/undefined');

    var calendarEl = document.getElementById('calendar');

    if (!calendarEl) {
        console.error('Calendar element not found!');
        return;
    }

    // Destroy existing calendar if present
    if (calendar) {
        calendar.destroy();
    }

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
        displayEventTime: false,
        eventClick: function (info) {
            console.log('Event clicked!');
            console.log('Event ID:', info.event.id);
            console.log('Event title:', info.event.title);

            info.jsEvent.preventDefault();
            const entryId = info.event.id;
            const url = `/journals/edit/${entryId}`;
            console.log('Navigating to:', url);

            window.location.href = url;
        },
        eventContent: function (arg) {
            return {
                html: '<div style="padding: 2px; overflow: hidden; cursor: pointer;">' + arg.event.title + '</div>'
            };
        }
    });

    calendar.render();
    console.log('Calendar rendered');
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
        calendar = null;
    }
};