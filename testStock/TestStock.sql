SET SQL DIALECT 3;

SET NAMES WIN1251;


SET TERM ^ ;

create or alter procedure check_stock_orea_by_barcorde (
  in_scan_type type of t_id,
  barcode   type of t_barcode,
  st_id     type of t_id,
  client_id type of t_id)
returns (
  id        type of t_id,
  scan_type type of t_id,
  prix_qty  type of t_quantity,
  stock_qty type of t_quantity,
  orea_qty  type of t_quantity)
AS
begin
  id = 1;
  scan_type = :in_scan_type;
  prix_qty = 0;
  stock_qty = 0;
  orea_qty = 0;
  if (:scan_type in (0, 2)) then
  begin
    select
      sum(s.qty_free),
      sum(s.qty_in)
      from skl s
        inner join catlogue c
              on (c.catlogue_id = s.catlogue_id)
      where (c.barcode = :barcode) and
            ((:st_id = 0) or ((:st_id > 0) and
            (s.st_id = :st_id)))
    into
      :stock_qty,
      :prix_qty;

    if (:stock_qty is null) then
      stock_qty = 0;
    if (:prix_qty is null) then
      prix_qty = 0;

  end

  if (:scan_type in (1, 2)) then
  begin
    select
      sum(o.qty)
      from orea o
        inner join skl s
              on (o.skl_id = s.skl_id)
        inner join catlogue c
              on (o.catlogue_id = c.catlogue_id)
      where (c.barcode = :barcode) and
            (o.client_id = :client_id) and
            (o.qty > 0) and
            ((:st_id = 0) or (:st_id > 0 and
            s.st_id = :st_id))
    into
      :orea_qty;

    if (:orea_qty is null) then
      orea_qty = 0;
  end

  suspend;
end^

SET TERM ; ^