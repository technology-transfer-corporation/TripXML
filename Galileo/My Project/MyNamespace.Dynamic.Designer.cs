using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Galileo.My
{
    internal static partial class MyProject
    {
        internal partial class MyWebServices
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public Galileo_wsGalileoCopy_XmlSelect m_Galileo_wsGalileoCopy_XmlSelect;

            public Galileo_wsGalileoCopy_XmlSelect Galileo_wsGalileoCopy_XmlSelect
            {
                [DebuggerHidden]
                get
                {
                    m_Galileo_wsGalileoCopy_XmlSelect = Create__Instance__<Galileo_wsGalileoCopy_XmlSelect>(m_Galileo_wsGalileoCopy_XmlSelect);
                    return m_Galileo_wsGalileoCopy_XmlSelect;
                }

                [DebuggerHidden]
                set
                {
                    if (object.ReferenceEquals(value, m_Galileo_wsGalileoCopy_XmlSelect))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__<Galileo_wsGalileoCopy_XmlSelect>(ref m_Galileo_wsGalileoCopy_XmlSelect);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Galileo_wsGalileoCopyIV_ImageViewer m_Galileo_wsGalileoCopyIV_ImageViewer;

            public Galileo_wsGalileoCopyIV_ImageViewer Galileo_wsGalileoCopyIV_ImageViewer
            {
                [DebuggerHidden]
                get
                {
                    m_Galileo_wsGalileoCopyIV_ImageViewer = Create__Instance__<Galileo_wsGalileoCopyIV_ImageViewer>(m_Galileo_wsGalileoCopyIV_ImageViewer);
                    return m_Galileo_wsGalileoCopyIV_ImageViewer;
                }

                [DebuggerHidden]
                set
                {
                    if (object.ReferenceEquals(value, m_Galileo_wsGalileoCopyIV_ImageViewer))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__<Galileo_wsGalileoCopyIV_ImageViewer>(ref m_Galileo_wsGalileoCopyIV_ImageViewer);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Galileo_wsGalileoProd_XmlSelect m_Galileo_wsGalileoProd_XmlSelect;

            public Galileo_wsGalileoProd_XmlSelect Galileo_wsGalileoProd_XmlSelect
            {
                [DebuggerHidden]
                get
                {
                    m_Galileo_wsGalileoProd_XmlSelect = Create__Instance__<Galileo_wsGalileoProd_XmlSelect>(m_Galileo_wsGalileoProd_XmlSelect);
                    return m_Galileo_wsGalileoProd_XmlSelect;
                }

                [DebuggerHidden]
                set
                {
                    if (object.ReferenceEquals(value, m_Galileo_wsGalileoProd_XmlSelect))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__<Galileo_wsGalileoProd_XmlSelect>(ref m_Galileo_wsGalileoProd_XmlSelect);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Galileo_wsGalileoProdIV_ImageViewer m_Galileo_wsGalileoProdIV_ImageViewer;

            public Galileo_wsGalileoProdIV_ImageViewer Galileo_wsGalileoProdIV_ImageViewer
            {
                [DebuggerHidden]
                get
                {
                    m_Galileo_wsGalileoProdIV_ImageViewer = Create__Instance__<Galileo_wsGalileoProdIV_ImageViewer>(m_Galileo_wsGalileoProdIV_ImageViewer);
                    return m_Galileo_wsGalileoProdIV_ImageViewer;
                }

                [DebuggerHidden]
                set
                {
                    if (object.ReferenceEquals(value, m_Galileo_wsGalileoProdIV_ImageViewer))
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__<Galileo_wsGalileoProdIV_ImageViewer>(ref m_Galileo_wsGalileoProdIV_ImageViewer);
                }
            }
        }
    }
}