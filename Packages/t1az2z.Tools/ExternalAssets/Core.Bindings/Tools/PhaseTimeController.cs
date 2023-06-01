using Core.Bindings;
using Core.Bindings.Tools.Helpers;

public enum PhaseControllerWorkMode {
    Once,
    OnceBack,
    Looped,
    LoopedBack
};

public enum PhaseControllerTimerMode {
    RealTime,
    ScaledTime,
    UnscaledTime
}

public class PhaseTimeController {
    public bool IsReversed;
    public PhaseControllerWorkMode WorkMode;
    public PhaseControllerTimerMode TimerMode;

    private float _phase = 0.0f;
    private double _lastRequested;
    private double _startTime = 0f;
    private double _timePassed = 0f;
    private double _length;

    public double Length {
        set => _length = Mathd.Max(0.001d, value);
        get => _length;
    }

    private bool _paused = true;

    public bool IsPaused {
        set {
            if (value) {
                _timePassed = GetTime() - _startTime;
                _paused = true;
                _active = false;
            }
            else {
                _paused = false;
                _startTime = GetTime() - _timePassed;
            }
        }

        get => _paused;
    }

    private bool _active = false;

    public bool IsActive => _active;

    private bool _finished = false;

    public bool IsFinished {
        get => _finished;

        set {
            if (value) {
                _paused = false;
                _active = false;
                _finished = true;
                _phase = 1f;
            }
            else {
                // nothing here
            }
        }
    }

    public double GetTime() {
        switch (TimerMode) {
            case PhaseControllerTimerMode.RealTime:
                return TimeTools.StaticRealTime;

            case PhaseControllerTimerMode.UnscaledTime:
                return TimeTools.DoubleTimeS;

            default:
                return TimeTools.DoubleTimeS_Scaled;
        }
    }

    public PhaseTimeController(PhaseControllerTimerMode timerMode, PhaseControllerWorkMode workMode, double length, bool suspended) {
        this.WorkMode = workMode;
        this.TimerMode = timerMode;
        _paused = suspended;
        _active = !suspended;
        Length = length;

        if (_active) {
            _startTime = GetTime();
        }
    }

    public void Play() {
        _active = true;
        _finished = false;
        if (_paused) {
            _startTime = GetTime() - _timePassed;
        }
        else {
            _startTime = GetTime();
            _timePassed = 0f;
        }

        _paused = false;
    }

    public void Reset() {
        _finished = false;
        _active = false;
        _paused = false;
        _phase = 0f;
        _lastRequested = 0f;
    }

    public float GetPhase() {
        var time = GetTime();
        if ((_active) && (time != _lastRequested)) {
            _phase = (float) ((time - _startTime) / _length);
            if (_phase > 1f) {
                switch (WorkMode) {
                    case PhaseControllerWorkMode.Once:
                        _active = false;
                        _finished = true;
                        _phase = 1f;
                        break;

                    case PhaseControllerWorkMode.OnceBack:
                        if (_phase > 2f) {
                            _active = false;
                            _finished = true;
                            _phase = 0f;
                        }
                        else {
                            _phase = 2f - _phase;
                        }

                        break;

                    case PhaseControllerWorkMode.Looped:
                        _phase -= (int) _phase;
                        break;

                    case PhaseControllerWorkMode.LoopedBack:
                        if (_phase > 2f) {
                            _phase -= (int) _phase;
                            _startTime = time - _phase * _length;
                        }
                        else {
                            _phase = 2f - _phase;
                        }

                        break;
                }
            }
        }

        _lastRequested = time;

        if (IsReversed) {
            return 1f - _phase;
        }
        else {
            return _phase;
        }
    }
}